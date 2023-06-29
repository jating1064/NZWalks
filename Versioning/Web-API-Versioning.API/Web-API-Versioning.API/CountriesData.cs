using Web_API_Versioning.API.Models.Domain;

namespace Web_API_Versioning.API
{
    public class CountriesData
    {
        public static List<Country> Get()
        {
            var countries = new[]
            {
                new{Id=1, Name="USA"},
                new{Id=2, Name="India"},
                new{Id=3, Name="UK"},
                new{Id=4, Name="Newzealand"},
                new{Id=5, Name="Singapore"}
            };

            return countries.Select(c=>new Country { Id=c.Id, Name=c.Name }).ToList();
        }
    }
}
