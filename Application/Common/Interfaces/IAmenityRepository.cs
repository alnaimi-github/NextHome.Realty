using NextHome.Realty.Domain.Entities;

namespace NextHome.Realty.Application.Common.Interfaces;

public interface IAmenityRepository : IRepository<Amenity>
{
    Task UpdateAsync(Amenity entity);
}