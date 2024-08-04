using NextHome.Realty.Application.Common.Dto;

namespace NextHome.Realty.Application.Services.Interfaces;

public interface IDashboardService
{
    Task<LineChartDto> GetMemberAndBookingLineChartData();
    Task<PieChartDto> GetBookingPieChartData();
    Task<RadialBarChartDto> GetTotalBookingRadialChartData();
    Task<RadialBarChartDto> GetRegisteredUserChartData();
    Task<RadialBarChartDto> GetRevenueChartData();
}