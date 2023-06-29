using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class AddRegionRequestDTO
    {
        [MinLength(3, ErrorMessage ="Code has to be a min of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be a max of 3 characters")]
        [Required]
        public string Code { get; set; }

        [MinLength(4, ErrorMessage = "Name has to be a min of 4 characters")]
        [MaxLength(10, ErrorMessage = "Name has to be a max of 10 characters")]
        [Required]
        public string Name { get; set; }
        
        public string? RegionImageURL { get; set; }

    }
}
