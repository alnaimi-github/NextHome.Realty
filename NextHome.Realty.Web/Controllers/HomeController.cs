using Microsoft.AspNetCore.Mvc;
using NextHome.Realty.Web.Models;
using System.Diagnostics;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Application.Common.Utility;
using NextHome.Realty.Web.ViewModels;

namespace NextHome.Realty.Web.Controllers
{
    public class HomeController(IUnitOfWork unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var homeVM = new HomeVM
            {
                VillaList = await unitOfWork.Villa.GetAllAsync(includeProperties: "VillaAmenity,VillaImages"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.UtcNow),
            };
            return View(homeVM);
        }
      
        [HttpPost]
        public async Task<IActionResult> GetVillasByDate(int nights,DateOnly checkInDate)
        {
            var villaList = await unitOfWork.Villa.GetAllAsync(includeProperties: "VillaAmenity,VillaImages");
            var villaNumbersList = (await unitOfWork.VillaNumber.GetAllAsync()).ToList();
            var bookedVillas = (await unitOfWork.Booking.GetAllAsync(u=>u.Status==SD.StatusPayment.Approved.ToString()||
                                                                       u.Status==SD.StatusPayment.CheckedIn.ToString())).ToList();
            foreach (var villa in villaList)
            {
                var roomsAvailable =
                    SD.VillaRoomsAvailable_Count(villa.Id, villaNumbersList, checkInDate, nights, bookedVillas);
                villa.IsAvailable= roomsAvailable > 0?true:false;
            }

            var homeVm = new HomeVM
            {
               VillaList = villaList!,
               CheckInDate = checkInDate,
               Nights = nights
            };
            return PartialView("_VillasListPartial",homeVm);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
