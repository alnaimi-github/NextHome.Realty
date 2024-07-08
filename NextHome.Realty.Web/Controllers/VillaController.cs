using Microsoft.AspNetCore.Mvc;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;
using NextHome.Realty.Persistence.Repository;

namespace NextHome.Realty.Web.Controllers
{
    public class VillaController(IVillaRepository villaRepository) : Controller
    {
        public IActionResult Index()
        {
            var villas = villaRepository.GetAll();
            return View(villas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (ModelState.IsValid)
            {
                villaRepository.Add(obj);
                villaRepository.Save();
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Update(int? VillaId)
        {
            var villa = villaRepository.Get(x => x.Id == VillaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid)
            {
                villaRepository.Update(obj);
                villaRepository.Save();
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Delete(int? VillaId)
        {
            var villa = villaRepository.Get(x => x.Id == VillaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            var villa = villaRepository.Get(x => x.Id == obj.Id);
            if (villa is not null)
            {
                villaRepository.Remove(villa);
                villaRepository.Save();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
