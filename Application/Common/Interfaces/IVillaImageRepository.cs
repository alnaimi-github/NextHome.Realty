using NextHome.Realty.Domain.Entities;

namespace NextHome.Realty.Application.Common.Interfaces
{
    public interface IVillaImageRepository : IRepository<VillaImage>
    {
        Task UpdateAsync(VillaImage obj);
    }
}
