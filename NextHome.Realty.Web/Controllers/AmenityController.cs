using System.Reflection;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Application.Common.Utility;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Web.ViewModels;
using NuGet.Protocol;
using TypeExtensions = System.Reflection.TypeExtensions;

namespace NextHome.Realty.Web.Controllers
{
    [Authorize(Roles = nameof(SD.RoleType.Admin))]
    public class AmenityController(IUnitOfWork unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var amenities = await unitOfWork.Amenity.GetAllAsync(includeProperties: "Villa");
            return View(amenities);
        }
        public async Task<IActionResult> Create()
        {
            var amenityVM = new AmenityVM
            {
                VillaList = (await unitOfWork.Villa.GetAllAsync())
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            };
            return View(amenityVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(AmenityVM obj)
        {
            var amenityExists = await unitOfWork.Amenity.AnyAsync(x => x.Id == obj.Amenity!.Id);
            if (ModelState.IsValid && !amenityExists)
            {
                await unitOfWork.Amenity.AddAsync(obj.Amenity!);
                await unitOfWork.SaveAsync();
                TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (amenityExists)
            {
                TempData["error"] = "The amenity already exists.";
            }
            var amenityVM = new AmenityVM
            {
                VillaList = (await unitOfWork.Villa.GetAllAsync())
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            };
            return View(amenityVM);
        }

        public async Task<IActionResult> Update(int? amenityId)
        {
            var amenityVM = new AmenityVM
            {
                VillaList = (await unitOfWork.Villa.GetAllAsync())
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Amenity = await unitOfWork.Amenity.GetAsync(x => x.Id == amenityId)
            };

            if (amenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(AmenityVM obj)
        {
            if (ModelState.IsValid)
            {
                await unitOfWork.Amenity.UpdateAsync(obj.Amenity!);
                await unitOfWork.SaveAsync();
                TempData["success"] = "The amenity has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public async Task<IActionResult> Delete(int? amenityId)
        {
            var amenityVM = new AmenityVM
            {
                VillaList = (await unitOfWork.Villa.GetAllAsync())
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Amenity = await unitOfWork.Amenity.GetAsync(x => x.Id == amenityId)
            };
            if (amenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(AmenityVM obj)
        {
            var amenity = await unitOfWork.Amenity.GetAsync(x => x.Id == obj.Amenity!.Id);
            if (amenity is not null)
            {
                await unitOfWork.Amenity.RemoveAsync(amenity);
                await unitOfWork.SaveAsync();
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
