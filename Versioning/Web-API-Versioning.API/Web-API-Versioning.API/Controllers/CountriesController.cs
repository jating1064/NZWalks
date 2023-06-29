using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning.API.Models.DTOs;

namespace Web_API_Versioning.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CountriesController : ControllerBase
    {
        [MapToApiVersion("1.0")]
        [HttpGet]
        public IActionResult GetV1()
        {
            var countriesDomainModel = CountriesData.Get();

            //Map DM to DTO

            var response = new List<CountryDTOV1>();
            foreach(var CountryDomain in countriesDomainModel)
            {
                response.Add(new CountryDTOV1()
                {
                    Id = CountryDomain.Id,
                    Name = CountryDomain.Name,
                });
            }
            return Ok(response);
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        public IActionResult GetV2()
        {
            var countriesDomainModel = CountriesData.Get();

            //Map DM to DTO

            var response = new List<CountryDTOV2>();
            foreach (var CountryDomain in countriesDomainModel)
            {
                response.Add(new CountryDTOV2()
                {
                    Id = CountryDomain.Id,
                    CountryName = CountryDomain.Name,
                });
            }
            return Ok(response);
        }
    }
}
