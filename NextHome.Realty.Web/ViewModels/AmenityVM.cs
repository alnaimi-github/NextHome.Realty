using Microsoft.AspNetCore.Mvc.Rendering;
using NextHome.Realty.Domain.Entities;

namespace NextHome.Realty.Web.ViewModels
{
    public class AmenityVM
    {
        public Amenity? Amenity { get; set; }
        public IEnumerable<SelectListItem>? VillaList { get; set; }
    }
}
