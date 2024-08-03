using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;

namespace NextHome.Realty.Persistence.Repository
{
    public class ApplicationUserRepository(ApplicationDbContext db)
        : Repository<ApplicationUser>(db), IApplicationUserRepository
    {

    }
}
