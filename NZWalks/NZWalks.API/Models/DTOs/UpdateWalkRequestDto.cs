using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class UpdateWalkRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name can be a max of 100 characters")]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = "Description can be a max of 1000 characters")]
        public string Description { get; set; }
        [Required]
        [Range(0, 50)]
        public Double LengthinKM { get; set; }

        public string? WalkImageURL { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }

        //public string DifficultyName { get; set; }
        //public string RegionName { get; set; }

        //Navigation Properties

        //public DifficultyDTO Difficulty { get; set; }

        //public RegionDTO Region { get; set; }
    }
}
