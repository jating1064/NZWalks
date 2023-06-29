using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.DTOs
{
    public class ImageUploadRequestDto
    {
        //[NotMapped]
        [Required]
        public IFormFile file { get; set; }

        [Required]
        public string FileName { get; set; }

        public string? FileDescription { get; set; }
    }
}
