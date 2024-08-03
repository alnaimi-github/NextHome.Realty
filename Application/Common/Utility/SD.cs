using NextHome.Realty.Domain.Entities;

namespace NextHome.Realty.Application.Common.Utility;

public static class SD
{
    public enum RoleType
    {
        Admin,
        Customer
    }

    public enum StatusPayment
    {
        Pending,
        Approved,
        CheckedIn,
        Completed,
        Cancelled,
        Refunded
    }

    public static int VillaRoomsAvailable_Count(int villaId,
        List<VillaNumber> villaNumberList, DateOnly checkInDate, int nights,
        List<Booking> bookings)
    {
        List<int> bookingInDate = new();
        int finalAvailableRoomForAllNights = int.MaxValue;
        var roomsInVilla = villaNumberList.Count(x => x.VillaId == villaId);

        for (var i = 0; i < nights; i++)
        {
            var villaIsBooked = bookings.Where(u => u.CheckInDate <= checkInDate.AddDays(i)
            &&u.CheckOutDate>=checkInDate.AddDays(i)&&u.VillaId==villaId);

            foreach (var booking in villaIsBooked)
            {
                if (!bookingInDate.Contains(booking.Id))
                {
                    bookingInDate.Add(booking.Id);
                }
            }

            var totalAvailableRooms = roomsInVilla - bookingInDate.Count();
            if (totalAvailableRooms==0)
            {
                return 0;
            }
            else
            {
                if (finalAvailableRoomForAllNights>totalAvailableRooms)
                {
                    finalAvailableRoomForAllNights = totalAvailableRooms;
                }
            }
        }
        return finalAvailableRoomForAllNights;
    }
}