using Microsoft.AspNetCore.Mvc;
using NextHome.Realty.Web.Models;
using System.Diagnostics;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Application.Common.Utility;
using NextHome.Realty.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel;
using Syncfusion.Presentation;
using ListType = Syncfusion.DocIO.DLS.ListType;

namespace NextHome.Realty.Web.Controllers
{
    public class HomeController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment) : Controller
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
        public async Task<IActionResult> GetVillasByDate(int nights,DateOnly checkInDate)
        {
            var villaList = await unitOfWork.Villa.GetAllAsync(includeProperties: "VillaAmenity,VillaImages");
            var villaNumbersList = (await unitOfWork.VillaNumber.GetAllAsync()).ToList();
            var bookedVillas = (await unitOfWork.Booking.GetAllAsync(u=>u.Status==SD.StatusPayment.Approved.ToString()||
                                                                       u.Status==SD.StatusPayment.CheckedIn.ToString())).ToList();
            foreach (var villa in villaList)
            {
                var roomsAvailable =
                    SD.VillaRoomsAvailable_Count(villa.Id, villaNumbersList, checkInDate, nights, bookedVillas);
                villa.IsAvailable= roomsAvailable > 0?true:false;
            }

            var homeVm = new HomeVM
            {
               VillaList = villaList!,
               CheckInDate = checkInDate,
               Nights = nights
            };
            return PartialView("_VillasListPartial",homeVm);
        }



        [HttpPost]
        public async Task<IActionResult> GeneratePPTExport(int id)
        {
            var villa = (await unitOfWork.Villa.GetAllAsync(includeProperties: "VillaAmenity,VillaImages")).FirstOrDefault(x=>x.Id==id);
            if (villa is null)
            {
                return RedirectToAction(nameof(Error));
            }

            string basePath = webHostEnvironment.WebRootPath;
            string filePath = basePath + @"/Exports/ExportVillaDetails.pptx";


            using IPresentation presentation = Presentation.Open(filePath);
            ISlide slide = presentation.Slides[0];


            IShape? shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaName") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = villa.Name;
            }

            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaDescription") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = villa.Description;
            }


            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtOccupancy") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("Max Occupancy : {0} adults", villa.Occupancy);
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaSize") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("Villa Size: {0} sqft", villa.Sqft);
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtPricePerNight") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("USD {0}/night", villa.Price.ToString("C"));
            }


            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaAmenitiesHeading") as IShape;
            if (shape is not null)
            {
                List<string> listItems = villa.VillaAmenity.Select(x => x.Name).ToList();

                shape.TextBody.Text = "";

                foreach (var item in listItems)
                {
                    IParagraph paragraph = shape.TextBody.AddParagraph();
                    ITextPart textPart = paragraph.AddTextPart(item);

                    paragraph.ListFormat.Type = (Syncfusion.Presentation.ListType)ListType.Bulleted;
                    paragraph.ListFormat.BulletCharacter = '\u2022';
                    textPart.Font.FontName = "system-ui";
                    textPart.Font.FontSize = 18;
                    textPart.Font.Color = ColorObject.FromArgb(144, 148, 152);
                }
            }

            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "imgVilla") as IShape;
            if (shape is not null)
            {
                byte[] imageData;
                string imageUrl;
                try
                {
                    var firstImage = villa.VillaImages?.FirstOrDefault();
                    imageUrl = string.Format("{0}{1}", basePath, firstImage!.ImageUrl);
                    imageData = System.IO.File.ReadAllBytes(imageUrl);
                }
                catch (Exception)
                {
                    imageUrl = string.Format("{0}{1}", basePath, "/Images/placeholder.png");
                    imageData = System.IO.File.ReadAllBytes(imageUrl);
                }
                slide.Shapes.Remove(shape);
                using MemoryStream imageStream = new(imageData);
                IPicture newPicture = slide.Pictures.AddPicture(imageStream, 60, 120, 300, 200);

            }



            MemoryStream memoryStream = new();
            presentation.Save(memoryStream);
            memoryStream.Position = 0;
            return File(memoryStream, "application/pptx", "villa.pptx");


        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
