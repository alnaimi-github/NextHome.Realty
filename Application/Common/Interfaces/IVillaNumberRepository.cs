using NextHome.Realty.Domain.Entities;

namespace NextHome.Realty.Application.Common.Interfaces
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        Task UpdateAsync(VillaNumber entity);
    }
}
