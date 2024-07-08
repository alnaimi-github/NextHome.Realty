using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;
using NextHome.Realty.Web.ViewModels;

namespace NextHome.Realty.Web.Controllers
{
    public class VillaNumberController(ApplicationDbContext db) : Controller
    {
        public IActionResult Index()
        {
            var villaNumbers = db.VillaNumbers.Include(x => x.Villa).ToList();
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            var villaNumberVM = new VillaNumberVM
            {
                VillaList = db.Villas.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            };
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            var roomNumberExists = db.VillaNumbers.Any(x => x.Villa_Number == obj.VillaNumber!.Villa_Number);
            if (ModelState.IsValid && !roomNumberExists)
            {
                db.Add(obj.VillaNumber!);
                db.SaveChanges();
                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The villa number already exists.";
            }
            var villaNumberVM = new VillaNumberVM
            {
                VillaList = db.Villas.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            };
            return View(villaNumberVM);
        }

        public IActionResult Update(int? VillaNumberId)
        {
            var villaNumberVM = new VillaNumberVM
            {
                VillaList = db.Villas.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                VillaNumber = db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == VillaNumberId)
            };

            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Update(VillaNumberVM obj)
        {
            if (ModelState.IsValid)
            {
                db.Update(obj.VillaNumber!);
                db.SaveChanges();
                TempData["success"] = "The villa number has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Delete(int? VillaNumberId)
        {
            var villaNumberVM = new VillaNumberVM
            {
                VillaList = db.Villas.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                VillaNumber = db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == VillaNumberId)
            };
            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM obj)
        {
            var villa = db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == obj.VillaNumber!.Villa_Number);
            if (villa is not null)
            {
                db.Remove(villa!);
                db.SaveChanges();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
