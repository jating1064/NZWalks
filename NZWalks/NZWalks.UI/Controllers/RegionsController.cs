using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {

            List<RegionDTO> response = new List<RegionDTO>();
            try
            {
                var client = httpClientFactory.CreateClient();
                //We can use this new client to consume web api

                var httpResponseMessage = await client.GetAsync("https://localhost:7212/api/Regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                //if 200
                //var stringResponseBody= 
                    response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());
                //ViewBag.stringResponseBody = stringResponseBody;
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();
            
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri= new Uri("https://localhost:7212/api/Regions"),
                Content = new StringContent(JsonSerializer.Serialize(model),Encoding.UTF8, "application/json")
            };

            var httpResponseMessage= await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response= await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

            if(response is not null) 
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var client = httpClientFactory.CreateClient();
            //We can use this new client to consume web api

            var response = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:7212/api/Regions/{Id.ToString()}");
            
            if(response is not null)
            {
                return View(response);
            }

            //ViewBag.Id = Id;
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDTO request)
        {
            var client = httpClientFactory.CreateClient();
            
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7212/api/Regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = 
                await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode() ;

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

            if(response is not null)
            {
                return RedirectToAction("Edit", "Regions");
            }
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDTO request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7212/api/Regions/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {

                throw;
            }

            return View("Edit");
        }
    }
}
