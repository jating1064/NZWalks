using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Mappings
{
    public class AutomapperProfiles:Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<Region, AddRegionRequestDTO>().ReverseMap();
            CreateMap<UpdateRegionRequestDTO,Region>().ReverseMap();
            CreateMap<AddWalksRequestDTO, Walk>().ReverseMap();
            CreateMap<WalkDTO,Walk>().ReverseMap();
            CreateMap<DifficultyDTO,Difficulty>().ReverseMap();
            CreateMap<UpdateWalkRequestDto,Walk>().ReverseMap();
        }
    }
}
