﻿using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;

namespace NextHome.Realty.Persistence.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public IVillaRepository Villa { get; }
    public IVillaNumberRepository VillaNumber { get; }
    public IVillaImageRepository VillaImage { get; }
    public IAmenityRepository Amenity { get; }
    public IBookingRepository Booking { get; }
    public IApplicationUserRepository ApplicationUser { get; }

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Villa = new VillaRepository(_db);
        VillaNumber = new VillaNumberRepository(_db);
        VillaImage = new VillaImageRepository(_db);
        Amenity = new AmenityRepository(_db);
        Booking = new BookingRepository(_db);
        ApplicationUser = new ApplicationUserRepository(_db);
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}