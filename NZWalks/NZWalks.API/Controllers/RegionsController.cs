using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;
using System.Net;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    // https://localhost:1234/api/regions
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDBContext dBContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDBContext dBContext, IRegionRepository regionRepository
            ,IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dBContext = dBContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //Get:https://localhost:portnumber/api/regions
        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            //logger.LogInformation("GetAll action method was invoked");
            //logger.LogWarning("Warning");
            //logger.LogError("Error");

            //try
            //{
            //    throw new Exception("Test Exception");
                var regionsDomain = await regionRepository.GetAllAsync();
                // var regions = await dBContext.Regions.ToListAsync();

                //Mapping DM to DTO
                //var regionDTO = new List<RegionDTO>();
                //foreach (var region in regionsDomain)
                //{
                //    regionDTO.Add(new RegionDTO()
                //    {
                //        Id = region.Id,
                //        Code = region.Code,
                //        Name = region.Name,
                //        RegionImageURL = region.RegionImageURL
                //    });
                //}
                logger.LogInformation($"Finished GetAll Regions Request with data: {JsonSerializer.Serialize(regionsDomain)}");

                var regionDTO = mapper.Map<List<RegionDTO>>(regionsDomain);


                return Ok(regionDTO);
        //}
            //catch(Exception ex)
            //{
            //    logger.LogError(ex, ex.Message);
            //    return Problem("Something went wrong", null, (int)HttpStatusCode.InternalServerError);
            //    throw;
            //}

    //{
    //    new Region
    //    {
    //        Id=Guid.NewGuid(),
    //        Name="Auckland Region",
    //        Code="Akl",
    //        RegionImageURL="Dummy.png"
    //    },
    //    new Region
    //    {
    //        Id=Guid.NewGuid(),
    //        Name="Wellington Region",
    //        Code="Akl",
    //        RegionImageURL="Dummy.png"
    //    }
    //};
    //return Ok(regions);
}

        //Get Single Region(Get Region by Id)
        //Get:

        //Get:https://localhost:portnumber/api/regions
        [HttpGet]
        //[Authorize(Roles ="Reader")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dBContext.Regions.Find(id);
            //var region = dBContext.Regions.SingleOrDefault(x=>x.Id==id);
            //var regionDomain = await dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map Domian Model to DTO

            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageURL = regionDomain.RegionImageURL
            //};
            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }

        //POST
        [HttpPost]
       // [Authorize(Roles ="Writer")]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //if (ModelState.IsValid)
            //{
                //Map/Convert the DTO to domain model

                //var regionDomainModel = new Region
                //{
                //    Code = addRegionRequestDTO.Code,
                //    Name = addRegionRequestDTO.Name,
                //    RegionImageURL = addRegionRequestDTO.RegionImageURL
                //};
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);
                //await dBContext.Regions.AddAsync(regionDomainModel);
                //await dBContext.SaveChangesAsync();

                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                //MapExtensions Domain Model To DTO

                //var regionDTO = new RegionDTO
                //{
                //    Id = regionDomainModel.Id,
                //    Code = regionDomainModel.Code,
                //    Name = regionDomainModel.Name,
                //    RegionImageURL = regionDomainModel.RegionImageURL
                //};
                var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);

            //}
            //else
            //{
            //    return BadRequest(ModelState);
            //}
        }
        //Update Region
        //PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        //[Authorize(Roles = "Writer")]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            //if (ModelState.IsValid)
            //{
                //check for the region by Id if exists

                //var regionDomainModel=await dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

                //map updateRegioNRequestDTO to regionDOmainModel

                //var regionDomainModel = new Region
                //{
                //    Code = updateRegionRequestDTO.Code,
                //    Name = updateRegionRequestDTO.Name,
                //    RegionImageURL = updateRegionRequestDTO.RegionImageURL
                //};
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDTO);

                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }
                //If region is found

                ////Map DTO to DomainModel

                //regionDomainModel.Code= updateRegionRequestDTO.Code;
                //regionDomainModel.RegionImageURL= updateRegionRequestDTO.RegionImageURL;
                //regionDomainModel.Name= updateRegionRequestDTO.Name;

                //// Save Changes
                //await dBContext.SaveChangesAsync();

                //Convert Domain Model to DTO

                //var regionDTO = new RegionDTO
                //{
                //    Id = regionDomainModel.Id,
                //    Code = regionDomainModel.Code,
                //    Name = regionDomainModel.Name,
                //    RegionImageURL = regionDomainModel.RegionImageURL
                //};


                return Ok(mapper.Map<RegionDTO>(regionDomainModel));

            //}
            //else
            //{
            //    return BadRequest(ModelState);
            //}
        }

        [HttpDelete]
        //[Authorize(Roles = "Writer")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            //Check if Region exists

            //var regionDomainModel= await dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var regionDomainModel=await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //if Region is found
            //Delete operation

            //dBContext.Regions.Remove(regionDomainModel);
            //await dBContext.SaveChangesAsync();

            //Map regionDomainModel to DTO

            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageURL = regionDomainModel.RegionImageURL
            //};

            var regionDTO= mapper.Map<RegionDTO>(regionDomainModel);
            return Ok(regionDTO);

        }
    }

}
