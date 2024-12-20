﻿using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;

namespace NextHome.Realty.Persistence.Repository
{
    public class VillaRepository(ApplicationDbContext db) : Repository<Villa>(db), IVillaRepository
    {
        public Task UpdateAsync(Villa entity)
        {
            db.Update(entity);
            return Task.CompletedTask;
        }
    }
}
