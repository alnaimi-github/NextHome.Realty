using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Web.Extensions;

namespace NextHome.Realty.Web.Controllers
{
    public class VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var villas = await unitOfWork.Villa.GetAllAsync();
            return View(villas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Villa obj, List<IFormFile>? files)
        {
            if (ModelState.IsValid)
            {
                await unitOfWork.Villa.AddAsync(obj);
                await unitOfWork.SaveAsync();
                var exitCreateVillaImageToFile = VillaImageSaveToFile.AddOrUpdateVillaImageToFileRoot(obj, files, webHostEnvironment.WebRootPath);
                if (exitCreateVillaImageToFile is not null)
                {
                    await unitOfWork.VillaImage.AddAsync(exitCreateVillaImageToFile);
                    await unitOfWork.SaveAsync();
                }
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Update(int? VillaId)
        {
            var villa = await unitOfWork.Villa.GetAsync(x => x.Id == VillaId, includeProperties: "VillaImages");
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Villa obj, List<IFormFile>? files)
        {
            if (ModelState.IsValid)
            {
                var exitUpdateVillaImageToFile = VillaImageSaveToFile.AddOrUpdateVillaImageToFileRoot(obj, files, webHostEnvironment.WebRootPath);
                if (exitUpdateVillaImageToFile is not null)
                {
                    await unitOfWork.VillaImage.UpdateAsync(exitUpdateVillaImageToFile);
                    await unitOfWork.SaveAsync();
                }
                await unitOfWork.Villa.UpdateAsync(obj);
                await unitOfWork.SaveAsync();
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public async Task<IActionResult> Delete(int? VillaId)
        {
            var villa = await unitOfWork.Villa.GetAsync(x => x.Id == VillaId, includeProperties: "VillaImages");
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Villa obj)
        {
            var villa = await unitOfWork.Villa.GetAsync(x => x.Id == obj.Id);
            if (villa is not null)
            {
                string productPath = @"VillaImages\Villas\villa-" + obj.Id;
                string finalPath = Path.Combine(webHostEnvironment.WebRootPath, productPath);

                if (Directory.Exists(finalPath))
                {
                    string[] filePaths = Directory.GetFiles(finalPath);
                    foreach (string filePath in filePaths)
                    {
                        System.IO.File.Delete(filePath);
                    }

                    Directory.Delete(finalPath);
                }
                await unitOfWork.Villa.RemoveAsync(villa);
                await unitOfWork.SaveAsync();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var imageToBeDeleted = await unitOfWork.VillaImage.GetAsync(u => u.Id == imageId);
            int villaId = imageToBeDeleted.VillaId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                                   Path.Combine(webHostEnvironment.WebRootPath,
                                   imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                await unitOfWork.VillaImage.RemoveAsync(imageToBeDeleted);
                await unitOfWork.SaveAsync();

                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Update), new { VillaId = villaId });
        }
    }
}
