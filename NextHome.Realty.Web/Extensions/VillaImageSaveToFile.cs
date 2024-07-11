using NextHome.Realty.Domain.Entities;

namespace NextHome.Realty.Web.Extensions
{
    public static class VillaImageSaveToFile
    {

        public static VillaImage AddOrUpdateVillaImageToFileRoot(Villa villa, List<IFormFile>? files, string wwwRootPath)
        {
            try
            {
                VillaImage villaImage = default!;
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string villaPath = @"VillaImages\villas\villa-" + villa.Id;
                        string finalPath = Path.Combine(wwwRootPath, villaPath);

                        if (!Directory.Exists(finalPath))
                            Directory.CreateDirectory(finalPath);

                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                      villaImage = new()
                        {
                            ImageUrl = @"\" + villaPath + @"\" + fileName,
                            VillaId = villa.Id,
                        };

                        if (villa.VillaImages == null)
                            villa.VillaImages = new List<VillaImage>();

                        villa.VillaImages.Add(villaImage);
                    }
                }
                return villaImage;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
