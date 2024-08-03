using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextHome.Realty.Domain.Entities;

namespace NextHome.Realty.Application.Common.Interfaces
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
    }
}
