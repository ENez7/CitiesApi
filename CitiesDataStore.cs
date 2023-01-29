using CityInfo.Api.Models;

namespace CityInfo.Api;

public class CitiesDataStore
{
    public List<CityDto> Cities { get; set; }
    public static CitiesDataStore Current { get; set; } = new();  // Singleton

    private CitiesDataStore()
    {
        Cities = new List<CityDto>
        {
            new()
            {
                Id = 1,
                Name = "New York City",
                Description = "City 1 description"
            },
            new()
            {
                Id = 2,
                Name = "Santa Cruz de la Sierra",
                Description = "City 2 description"
            },
            new()
            {
                Id = 3,
                Name = "Paris",
                Description = "City 3 description"
            }
        };
    }
}