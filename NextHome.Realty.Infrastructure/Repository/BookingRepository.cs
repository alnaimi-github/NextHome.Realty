using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Application.Common.Utility;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;

namespace NextHome.Realty.Persistence.Repository
{
    public class BookingRepository(ApplicationDbContext db) :Repository<Booking>(db),IBookingRepository
    {
        public async Task UpdateAsync(Booking entity)
        {
            await Task.FromResult(db.Update(entity));
        }

        public async Task UpdateStatus(int bookingId, string bookingStatus,int villaNumber=0)
        {
            var bookingFromDb =await db.Bookings.FirstOrDefaultAsync(u => u.Id == bookingId);
            if (bookingFromDb!=null)
            {
                bookingFromDb!.Status = bookingStatus;
                if (bookingStatus == SD.StatusPayment.CheckedIn.ToString())
                {
                    bookingFromDb.VillaNumber = villaNumber;
                   bookingFromDb.ActualCheckInDate=DateTime.UtcNow; 
                }
                if (bookingStatus == SD.StatusPayment.Completed.ToString())
                {
                    bookingFromDb.ActualCheckOutDate = DateTime.UtcNow;
                }
            }
        }

        public async Task UpdateStripePaymentId(int bookingId, string sessionId, string paymentIntentId)
        {
            var bookingFromDb = await db.Bookings.FirstOrDefaultAsync(u => u.Id == bookingId);
            if (bookingFromDb != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    bookingFromDb.StripeSessionId=sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    bookingFromDb.StripePaymentIntentId = paymentIntentId;
                    bookingFromDb.PaymentDate=DateTime.UtcNow;
                    bookingFromDb.IsPaymentSuccessful = true;
                }
            }
        }
    }
}
