using Microsoft.AspNetCore.Mvc;
using NextHome.Realty.Application.Services.Interfaces;

namespace NextHome.Realty.Web.Controllers;

public class DashboardController(IDashboardService dashboardService) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetTotalBookingRadialChartData()
    {
        return Json(await dashboardService.GetTotalBookingRadialChartData());
    }

    public async Task<IActionResult> GetRegisteredUserChartData()
    {
        return Json(await dashboardService.GetRegisteredUserChartData());
    }

    public async Task<IActionResult> GetRevenueChartData()
    {
        return Json(await dashboardService.GetRevenueChartData());
    }


    public async Task<IActionResult> GetBookingPieChartData()
    {
        return Json(await dashboardService.GetBookingPieChartData());
    }

    public async Task<IActionResult> GetMemberAndBookingLineChartData()
    {
        return Json(await dashboardService.GetMemberAndBookingLineChartData());
    }
}