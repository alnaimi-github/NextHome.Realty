using Microsoft.AspNetCore.Mvc;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;

namespace NextHome.Realty.Web.Controllers
{
    public class VillaController(ApplicationDbContext db) : Controller
    {
        public IActionResult Index()
        {
            var villas = db.Villas.ToList();
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
                db.Add(obj);
                db.SaveChanges();
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Update(int? VillaId)
        {
            var villa = db.Villas.FirstOrDefault(x => x.Id == VillaId);
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
                db.Update(obj);
                db.SaveChanges();
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Delete(int? VillaId)
        {
            var villa = db.Villas.FirstOrDefault(x => x.Id == VillaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            var villa = db.Villas.FirstOrDefault(x => x.Id == obj.Id);
            if (villa is not null)
            {
                db.Remove(villa!);
                db.SaveChanges();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
