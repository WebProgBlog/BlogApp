using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace BlogApp.Helpers
{
    public class ImageHelper
    {
        public static async Task<string> SaveResizedImageAsync(IFormFile imageFile, string wwwRootPath, int width, int height)
        {
            var uploadsFolder = Path.Combine(wwwRootPath, "img", "blogs");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var image = await Image.LoadAsync(imageFile.OpenReadStream());
            image.Mutate(x => x.Resize(width, height));
            await image.SaveAsJpegAsync(filePath, new JpegEncoder());

            return "/img/blogs/" + uniqueFileName;
        }
    }
}
