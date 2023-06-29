using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTOs
{
    public class WalkDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Double LengthinKM { get; set; }
        public string? WalkImageURL { get; set; }

        public Guid DifficultyId { get; set; }

        public Guid RegionId { get; set; }

        //public string DifficultyName { get; set;}
        //public string RegionName { get; set; }

        //Navigation Properties

        public DifficultyDTO Difficulty { get; set; }

        public RegionDTO Region { get; set; }
    }
}
