using Microsoft.EntityFrameworkCore;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;
using System.Linq.Expressions;

namespace NextHome.Realty.Persistence.Repository
{
    public class VillaRepository(ApplicationDbContext db) : IVillaRepository
    {
        public void Add(Villa entity)
        {
            db.Add(entity);
        }

        public Villa Get(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Villa> query = db.Set<Villa>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var include in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include);
                }
            }

            return query.FirstOrDefault()!;
        }

        public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Villa> query = db.Set<Villa>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var include in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include);
                }
            }

            return query.ToList();
        }

        public void Remove(Villa entity)
        {
            db.Remove(entity);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Villa entity)
        {
            db.Update(entity);
        }
    }
}
