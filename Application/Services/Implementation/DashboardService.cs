﻿using NextHome.Realty.Application.Common.Dto;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Application.Common.Utility;
using NextHome.Realty.Application.Services.Interfaces;

namespace NextHome.Realty.Application.Services.Implementation;

public class DashboardService(IUnitOfWork unitOfWork) : IDashboardService
{
    private static readonly int PreviousMonth = DateTime.UtcNow.Month == 1 ? 12 : DateTime.UtcNow.Month - 1;
    private readonly DateTime _previousMonthStartDate = new(DateTime.UtcNow.Year, PreviousMonth, 1);
    private readonly DateTime _currentMonthStartDate = new(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

    public async Task<PieChartDto> GetBookingPieChartData()
    {
        var totalBookings = await unitOfWork.Booking.GetAllAsync(u => u.BookingDate >= DateTime.UtcNow.AddDays(-30) &&
                                                                      (u.Status != SD.StatusPayment.Pending.ToString()
                                                                       || u.Status == SD.StatusPayment.Cancelled
                                                                           .ToString()));
        var customerWithOneBooking =
            totalBookings.GroupBy(b => b.UserId).Where(x => x.Count() == 1).Select(x => x.Key).ToList();
        var bookingsByNewCustomer = customerWithOneBooking.Count();
        var bookingByReturningCustomer = totalBookings.Count() - bookingsByNewCustomer;

        var pieChartDto = new PieChartDto
        {
            Labels = new[] { "New Customer Bookings", "Returning Customer Bookings" },
            Series = new decimal[] { bookingsByNewCustomer, bookingByReturningCustomer }
        };
        return pieChartDto;
    }

    public async Task<LineChartDto> GetMemberAndBookingLineChartData()
    {
        var bookingData = (await unitOfWork.Booking.GetAllAsync(u =>
                u.BookingDate >= DateTime.Now.AddDays(-30) && u.BookingDate.Date <= DateTime.Now))
            .GroupBy(b => b.BookingDate.Date)
            .Select(u => new
            {
                DateTime = u.Key,
                NewBookingCount = u.Count()
            });

        var customerData = (await unitOfWork.ApplicationUser.GetAllAsync(u =>
                u.CreatedAt >= DateTime.Now.AddDays(-30) && u.CreatedAt.Date <= DateTime.Now))
            .GroupBy(b => b.CreatedAt.Date)
            .Select(u => new
            {
                DateTime = u.Key,
                NewCustomerCount = u.Count()
            });

        var leftJoin = bookingData.GroupJoin(customerData, booking => booking.DateTime, customer => customer.DateTime,
            (booking, customer) => new
            {
                booking.DateTime,
                booking.NewBookingCount,
                NewCustomerCount = customer.Select(x => x.NewCustomerCount).FirstOrDefault()
            });

        var rightJoin = customerData.GroupJoin(bookingData, customer => customer.DateTime, booking => booking.DateTime,
            (customer, booking) => new
            {
                customer.DateTime,
                NewBookingCount = booking.Select(x => x.NewBookingCount).FirstOrDefault(),
                customer.NewCustomerCount
            });

        var mergedData = leftJoin.Union(rightJoin).OrderBy(x => x.DateTime).ToList();

        var newBookingData = mergedData.Select(x => x.NewBookingCount).ToArray();
        var newCustomerData = mergedData.Select(x => x.NewCustomerCount).ToArray();
        var categories = mergedData.Select(x => x.DateTime.ToString("MM/dd/yyyy")).ToArray();

        List<ChartData> chartDataList = new()
        {
            new ChartData
            {
                Name = "New Bookings",
                Data = newBookingData
            },
            new ChartData
            {
                Name = "New Members",
                Data = newCustomerData
            }
        };

        LineChartDto lineChartDto = new()
        {
            Categories = categories,
            Series = chartDataList
        };
        return lineChartDto;
    }

    public async Task<RadialBarChartDto> GetRegisteredUserChartData()
    {
        var totalUsers = await unitOfWork.ApplicationUser.GetAllAsync();
        var countByCurrentMonth = totalUsers.Count(u => u.CreatedAt >= _currentMonthStartDate
                                                        && u.CreatedAt <= DateTime.UtcNow);

        var countByPreviousMonth = totalUsers.Count(u => u.CreatedAt >= _previousMonthStartDate
                                                         && u.CreatedAt <= _currentMonthStartDate);

        var returnUsers = GetRadialChartDataModel(totalUsers.Count(), countByCurrentMonth, countByPreviousMonth);
        return returnUsers;
    }

    public async Task<RadialBarChartDto> GetRevenueChartData()
    {
        var totalBookings = await unitOfWork.Booking.GetAllAsync(u => u.Status != SD.StatusPayment.Pending.ToString()
                                                                      || u.Status ==
                                                                      SD.StatusPayment.Cancelled.ToString());
        var totalRevenue = Convert.ToInt32(totalBookings.Sum(u => u.TotalCost));
        var countByCurrentMonth = totalBookings.Where(u => u.BookingDate >= _currentMonthStartDate
                                                           && u.BookingDate <= DateTime.UtcNow).Sum(u => u.TotalCost);

        var countByPreviousMonth = totalBookings.Where(u => u.BookingDate >= _previousMonthStartDate
                                                            && u.BookingDate <= _currentMonthStartDate)
            .Sum(u => u.TotalCost);

        var returnRevenues = GetRadialChartDataModel(totalRevenue, countByCurrentMonth, countByPreviousMonth);
        return returnRevenues;
    }

    public async Task<RadialBarChartDto> GetTotalBookingRadialChartData()
    {
        var totalBookings = await unitOfWork.Booking.GetAllAsync(u => u.Status != SD.StatusPayment.Pending.ToString()
                                                                      || u.Status ==
                                                                      SD.StatusPayment.Cancelled.ToString());
        var countByCurrentMonth = totalBookings.Count(u => u.BookingDate >= _currentMonthStartDate
                                                           && u.BookingDate <= DateTime.UtcNow);

        var countByPreviousMonth = totalBookings.Count(u => u.BookingDate >= _previousMonthStartDate
                                                            && u.BookingDate <= _currentMonthStartDate);
        var returnBooks = GetRadialChartDataModel(totalBookings!.Count(), countByCurrentMonth, countByPreviousMonth);
        return returnBooks;
    }


    private static RadialBarChartDto GetRadialChartDataModel(int totalCount, double currentMonthCount,
        double prevMonthCount)
    {
        var radialBarChartVm = new RadialBarChartDto();

        var increaseDecreaseRatio = 100;

        if (prevMonthCount != 0)
            increaseDecreaseRatio = Convert.ToInt32((currentMonthCount - prevMonthCount) / prevMonthCount * 100);

        radialBarChartVm.TotalCount = totalCount;
        radialBarChartVm.CountInCurrentMonth = Convert.ToInt32(currentMonthCount);
        radialBarChartVm.HasRatioIncreased = currentMonthCount > prevMonthCount;
        radialBarChartVm.Series = new[] { increaseDecreaseRatio };
        return radialBarChartVm;
    }
}