using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    //api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        //Create Walk
        //Post:/api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDTO 
            addWalksRequestDTO)
        {
            //if (ModelState.IsValid)
            //{
                var walksDomainModel = mapper.Map<Walk>(addWalksRequestDTO);
                await walkRepository.CreateAsync(walksDomainModel);
                return Ok(mapper.Map<WalkDTO>(walksDomainModel));
            //}
            //else
            //{
            //    return BadRequest(ModelState);
            //}
        }

        //Get Walks
        //Get:/api/Walks?filterOn=Name & filterQuery=Track & sortBy=Name & \
        //isAscending = true & pageNumber=1 &pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string? filterOn, 
            [FromQuery] string? filterQuery, [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending, [FromQuery] int pageNumber=1,
            [FromQuery] int pageSize = 100)
        {
            var walksDomainModel= await walkRepository.GetAllAsync(filterOn,filterQuery, 
                sortBy,isAscending ?? true, pageNumber, pageSize);
            throw new Exception("Test exception from Walks Controller");
            return Ok(mapper.Map<List<WalkDTO>>(walksDomainModel));
        }

        //Get Walk by Id
        //Get:/api/Walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel=await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //Map Walk Domain model to Walk DTO

            return Ok(mapper.Map<WalkDTO>(walkDomainModel));

        }

        //Update Walk by Id
        //Put:/api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            //if (ModelState.IsValid)
            //{
                //Map DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                //Map DM to DTO
                return Ok(mapper.Map<WalkDTO>(walkDomainModel));
            //}
            //else
            //{
            //    return BadRequest(ModelState);
            //}

        }
        //Delete a walk by ID
        //DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var DeletedwalkDomainModel= await walkRepository.DeleteAsync(id);

            if (DeletedwalkDomainModel == null)
            {
                return NotFound();
            }
            //Map Domain model to DTO
            return Ok(mapper.Map < WalkDTO >(DeletedwalkDomainModel));
        }
    }
}
