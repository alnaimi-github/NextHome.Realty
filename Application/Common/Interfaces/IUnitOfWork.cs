﻿namespace NextHome.Realty.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IVillaRepository Villa { get; }
    IVillaNumberRepository VillaNumber { get; }
    IVillaImageRepository VillaImage { get; }
    IAmenityRepository Amenity { get; }
    IBookingRepository Booking { get; }
    IApplicationUserRepository ApplicationUser { get; }

    Task SaveAsync();
}