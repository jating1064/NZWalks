using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironement;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDBContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironement, IHttpContextAccessor httpContextAccessor,
            NZWalksDBContext dbContext)
        {
            this.webHostEnvironement = webHostEnvironement;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironement.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            //FileStream Object to copy this File(IFormFile)

            using var stream = new FileStream(localFilePath, FileMode.Create);

            await image.file.CopyToAsync(stream);

            //Save changes to db

            //https://localhost:1234/images/image.jpg

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}:" +
                $"//{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            //Saving
            //Add image to the images table

            await dbContext.Images.AddAsync(image);

            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}
