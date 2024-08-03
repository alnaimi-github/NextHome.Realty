﻿using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Application.Common.Utility;
using NextHome.Realty.Domain.Entities;
using Stripe.Checkout;

namespace NextHome.Realty.Web.Controllers;

[Authorize]
public class BookingController(IUnitOfWork unitOfWork) : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> FinalizeBooking(int villaId, DateOnly checkInDate, int nights)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var applicationUser = await unitOfWork.ApplicationUser.GetAsync(u => u.Id == userId);
        var booking = new Booking
        {
            VillaId = villaId,
            CheckInDate = checkInDate,
            Nights = nights,
            CheckOutDate = checkInDate.AddDays(nights),
            Villa = await unitOfWork.Villa.GetAsync(u => u.Id == villaId, "VillaAmenity,VillaImages"),
            UserId = userId,
            Phone = applicationUser.PhoneNumber,
            Name = applicationUser.Name,
            Email = applicationUser.Email!
        };
        booking.TotalCost = booking.Villa.Price * nights;
        return View(booking);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FinalizeBooking(Booking booking)
    {
        var villa = await unitOfWork.Villa.GetAsync(u => u.Id == booking.VillaId);
        booking.TotalCost = villa.Price * booking.Nights;
        booking.Status = SD.StatusPayment.Pending.ToString();
        booking.BookingDate = DateTime.UtcNow;

        var villaNumbersList = (await unitOfWork.VillaNumber.GetAllAsync()).ToList();
        var bookedVillas = (await unitOfWork.Booking.GetAllAsync(u => u.Status == SD.StatusPayment.Approved.ToString() ||
                                                                      u.Status == SD.StatusPayment.CheckedIn.ToString())).ToList();

            var roomsAvailable =
                SD.VillaRoomsAvailable_Count(villa.Id, villaNumbersList, booking.CheckInDate,booking.Nights, bookedVillas);
            if (roomsAvailable==0)
            {
                TempData["error"] = "Room has been sold out!";
            //no rooms available
            return RedirectToAction(nameof(FinalizeBooking), new
            {
                villaId = booking.VillaId,
                checkInDate = booking.CheckInDate,
                nights = booking.Nights
            });
            }


        await unitOfWork.Booking.AddAsync(booking);
        await unitOfWork.SaveAsync();

        var domain = Request.Scheme + "://" + Request.Host.Value + "/";
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
            SuccessUrl = domain + $"Booking/BookingConfirmation?bookingId={booking.Id}",
            CancelUrl = domain +
                        $"Booking/FinalizeBooking?villaId={booking.VillaId}&checkInDate={booking.CheckInDate}&nights={booking.Nights}",
        };

        options.LineItems.Add(new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long)(booking.TotalCost * 100),
                Currency = "usd",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = villa.Name,
                }
            },
            Quantity = 1
        });

        var service = new SessionService();
        Session session = service.Create(options);
        await unitOfWork.Booking.UpdateStripePaymentId(booking.Id, session.Id, session.PaymentIntentId);
        await unitOfWork.SaveAsync();
        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);
    }

    public async Task<IActionResult> BookingConfirmation(int bookingId)
    {
        var bookingFromDb = await unitOfWork.Booking.GetAsync(u => u.Id == bookingId, includeProperties: "User,Villa");

        if (bookingFromDb.Status==SD.StatusPayment.Pending.ToString())
        {
            var service = new SessionService();
            Session session = await service.GetAsync(bookingFromDb.StripeSessionId);
            if (session.PaymentStatus=="paid")
            {
                await unitOfWork.Booking.UpdateStatus(bookingFromDb.Id, SD.StatusPayment.Approved.ToString());
                await unitOfWork.Booking.UpdateStripePaymentId(bookingFromDb.Id, session.Id,session.PaymentIntentId);
                await unitOfWork.SaveAsync();
            }
        }
        return View(bookingId);
    }


    public async Task<IActionResult> BookingDetails(int bookingId)
    {
        var bookingFromDb = await unitOfWork.Booking.GetAsync(
            u => u.Id == bookingId,
            includeProperties: "User,Villa");

        if (bookingFromDb == null)
        {
            return NotFound();
        }

        if (bookingFromDb.VillaNumber == 0 && bookingFromDb.Status == SD.StatusPayment.Approved.ToString())
        {
            var availableVillaNumbers = await AssignAvailableVillaNumberByVilla(bookingFromDb.VillaId);

            var villaNumbers = await unitOfWork.VillaNumber.GetAllAsync(u =>
                u.VillaId == bookingFromDb.VillaId &&
                availableVillaNumbers.Contains(u.Villa_Number));

            bookingFromDb.VillaNumbers = villaNumbers.ToList();
        }
        return View(bookingFromDb);
    }

    [HttpPost]
    [Authorize(Roles = nameof(SD.RoleType.Admin))]
    public async Task<IActionResult> CheckIn(Booking booking)
    {
        await unitOfWork.Booking.UpdateStatus(booking.Id, SD.StatusPayment.CheckedIn.ToString(), booking.VillaNumber);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Booking Updated successfully!";
        return RedirectToAction(nameof(BookingDetails),new{bookingId=booking.Id});
    }
    [HttpPost]
    [Authorize(Roles = nameof(SD.RoleType.Admin))]
    public async Task<IActionResult> CheckOut(Booking booking)
    {
        await unitOfWork.Booking.UpdateStatus(booking.Id, SD.StatusPayment.Completed.ToString(), booking.VillaNumber);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Booking Completed successfully!";
        return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
    }
    [HttpPost]
    [Authorize(Roles = nameof(SD.RoleType.Admin))]
    public async Task<IActionResult> CancelBooking(Booking booking)
    {
        await unitOfWork.Booking.UpdateStatus(booking.Id, SD.StatusPayment.Cancelled.ToString(), 0);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Booking Canceled successfully!";
        return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
    }

    private async Task<List<int>> AssignAvailableVillaNumberByVilla(int villaId)
    {

        var villaNumbers = await unitOfWork.VillaNumber.GetAllAsync(u => u.VillaId == villaId);

        var checkedInVillaNumbers = (await unitOfWork.Booking.GetAllAsync(u =>
                u.VillaId == villaId && u.Status == SD.StatusPayment.CheckedIn.ToString()))
            .Select(u => u.VillaNumber)
            .ToHashSet();

        return villaNumbers
            .Where(vn => !checkedInVillaNumbers.Contains(vn.Villa_Number))
            .Select(vn => vn.Villa_Number)
            .ToList();
    }


    #region Call API DataTable
    [HttpGet]
    public async Task<IActionResult> GetAll(string status)
    {
        IEnumerable<Booking> objBookings;
        if (User.IsInRole(SD.RoleType.Admin.ToString()))
        {
            objBookings=await unitOfWork.Booking.GetAllAsync(includeProperties:"User,Villa");
        }
        else
        {
            var claimIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            objBookings = await unitOfWork.Booking.GetAllAsync(u=>u.UserId==userId,includeProperties:"User,Villa");
        }

        if (!string.IsNullOrEmpty(status) && status != "all")
        {
            objBookings = objBookings.Where(u => u.Status!.ToLower()==(status.ToLower()));
        }
        return Json(new { data = objBookings });
    }

    #endregion
}