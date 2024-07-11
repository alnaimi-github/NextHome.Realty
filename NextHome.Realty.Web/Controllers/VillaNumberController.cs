using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Web.ViewModels;

namespace NextHome.Realty.Web.Controllers
{
    public class VillaNumberController(IUnitOfWork unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var villaNumbers = await unitOfWork.VillaNumber.GetAllAsync(includeProperties: "Villa");
            return View(villaNumbers);
        }
        public async Task<IActionResult> Create()
        {
            var villaNumberVM = new VillaNumberVM
            {
                VillaList = (await unitOfWork.Villa.GetAllAsync())
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            };
            return View(villaNumberVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(VillaNumberVM obj)
        {
            var roomNumberExists = await unitOfWork.VillaNumber.AnyAsync(x => x.Villa_Number == obj.VillaNumber!.Villa_Number);
            if (ModelState.IsValid && !roomNumberExists)
            {
                await unitOfWork.VillaNumber.AddAsync(obj.VillaNumber!);
                await unitOfWork.SaveAsync();
                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The villa number already exists.";
            }
            var villaNumberVM = new VillaNumberVM
            {
                VillaList = (await unitOfWork.Villa.GetAllAsync())
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            };
            return View(villaNumberVM);
        }

        public async Task<IActionResult> Update(int? VillaNumberId)
        {
            var villaNumberVM = new VillaNumberVM
            {
                VillaList = (await unitOfWork.Villa.GetAllAsync())
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                VillaNumber = await unitOfWork.VillaNumber.GetAsync(x => x.Villa_Number == VillaNumberId)
            };

            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(VillaNumberVM obj)
        {
            if (ModelState.IsValid)
            {
                await unitOfWork.VillaNumber.UpdateAsync(obj.VillaNumber!);
                await unitOfWork.SaveAsync();
                TempData["success"] = "The villa number has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public async Task<IActionResult> Delete(int? VillaNumberId)
        {
            var villaNumberVM = new VillaNumberVM
            {
                VillaList = (await unitOfWork.Villa.GetAllAsync())
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                VillaNumber = await unitOfWork.VillaNumber.GetAsync(x => x.Villa_Number == VillaNumberId)
            };
            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(VillaNumberVM obj)
        {
            var villa = await unitOfWork.VillaNumber.GetAsync(x => x.Villa_Number == obj.VillaNumber!.Villa_Number);
            if (villa is not null)
            {
                await unitOfWork.VillaNumber.RemoveAsync(villa!);
                await unitOfWork.SaveAsync();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
