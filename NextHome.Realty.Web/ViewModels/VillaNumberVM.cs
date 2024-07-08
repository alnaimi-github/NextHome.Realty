using Microsoft.AspNetCore.Mvc.Rendering;
using NextHome.Realty.Domain.Entities;

namespace NextHome.Realty.Web.ViewModels
{
    public class VillaNumberVM
    {
        public VillaNumber? VillaNumber { get; set; }
        public IEnumerable<SelectListItem>?VillaList { get; set; }
    }
}
