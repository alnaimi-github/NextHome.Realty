using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;

namespace NextHome.Realty.Persistence.Repository
{
    public class VillaNumberRepository(ApplicationDbContext db) :
        Repository<VillaNumber>(db), IVillaNumberRepository
    {
        public Task UpdateAsync(VillaNumber entity)
        {
            db.Update(entity);
            return Task.CompletedTask;
        }
    }
}
