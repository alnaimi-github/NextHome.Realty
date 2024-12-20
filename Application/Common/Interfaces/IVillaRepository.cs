﻿using NextHome.Realty.Domain.Entities;

namespace NextHome.Realty.Application.Common.Interfaces
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task UpdateAsync(Villa entity);
    }
}
