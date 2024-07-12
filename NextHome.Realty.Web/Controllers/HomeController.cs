using Microsoft.AspNetCore.Mvc;
using NextHome.Realty.Web.Models;
using System.Diagnostics;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Web.ViewModels;

namespace NextHome.Realty.Web.Controllers
{
    public class HomeController(IUnitOfWork unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var homeVM = new HomeVM
            {
                VillaList = await unitOfWork.Villa.GetAllAsync(includeProperties: "VillaAmenity,VillaImages"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.UtcNow),
            };
            return View(homeVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(HomeVM homeVm)
        {
            homeVm.VillaList = await unitOfWork.Villa.GetAllAsync(includeProperties: "VillaAmenity,VillaImages");
            return View(homeVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetVillasByDate(int nights,DateOnly checkInDate)
        {
            var villaList = await unitOfWork.Villa.GetAllAsync(includeProperties: "VillaAmenity,VillaImages");
            foreach (var villa in villaList)
            {
                if (villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }
            }

            var homeVm = new HomeVM
            {
               VillaList = villaList!,
               CheckInDate = checkInDate,
               Nights = nights
            };
            return PartialView("_VillasListPartial",homeVm);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
