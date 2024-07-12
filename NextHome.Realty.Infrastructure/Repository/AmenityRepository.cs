using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;

namespace NextHome.Realty.Persistence.Repository;

public class AmenityRepository(ApplicationDbContext db) : Repository<Amenity>(db), IAmenityRepository
{
    public Task UpdateAsync(Amenity entity)
    {
        db.Update(entity);
        return Task.CompletedTask;
    }
}