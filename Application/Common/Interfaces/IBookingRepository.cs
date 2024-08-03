using NextHome.Realty.Domain.Entities;

namespace NextHome.Realty.Application.Common.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task UpdateAsync(Booking entity);
    Task UpdateStatus(int bookingId,string bookingStatus,int villaNumber=0);
    Task UpdateStripePaymentId(int bookingId,string sessionId,string paymentIntentId);
}