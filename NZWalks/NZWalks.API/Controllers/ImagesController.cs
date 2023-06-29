using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);
            if (ModelState.IsValid)
            {
                //Use repository to upload image
                //Convert DTO to DM
                var imageDomainModel = new Image
                {
                    file = request.file,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                    FileExtension = Path.GetExtension(request.file.FileName),
                    FileSizeInBytes = request.file.Length
                };
                //FilePath will be added from Repository
                //Use Repository to upload image
                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg", ".jfif" };
            if(allowedExtensions.Contains(Path.GetExtension(request.file.FileName)) ==false)
            {
                //Add errors to Model State
                ModelState.AddModelError("file", "Unsupported File Extension");            
            }
            //10 MB
            if(request.file.Length > 10485760) 
            {
                ModelState.AddModelError("file", "File Size greater than 10 MB isn't supported");
            }
        }

    }
}
