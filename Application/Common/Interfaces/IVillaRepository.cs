using NextHome.Realty.Domain.Entities;
using System.Linq.Expressions;

namespace NextHome.Realty.Application.Common.Interfaces
{
    public interface IVillaRepository
    {
        IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null,string? includeProperties=null);
        Villa Get(Expression<Func<Villa, bool>>? filter = null,string? includeProperties=null);
        void Add(Villa entity);
        void Update(Villa entity);
        void Remove(Villa entity);
        void Save();
    }
}
