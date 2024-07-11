using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;

namespace NextHome.Realty.Persistence.Repository
{
    public class VillaImageRepository(ApplicationDbContext db) : Repository<VillaImage>(db), IVillaImageRepository
    {
        public async Task UpdateAsync(VillaImage obj)
        {
            await Task.FromResult(db.Update(obj));
        }
    }
}
