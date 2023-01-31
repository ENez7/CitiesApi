using CityInfo.Api.Models;

namespace CityInfo.Api;

public class CitiesDataStore
{
    public List<CityDto> Cities { get; set; }
    // public static CitiesDataStore Current { get; } = new();  // Singleton

    public CitiesDataStore()
    {
        Cities = new List<CityDto>
        {
            new()
            {
                Id = 1,
                Name = "New York City",
                Description = "City 1 description",
                PointsOfInterest = new List<PointOfInterestDto>
                {
                    new ()
                    {
                        Id =  1,
                        Name = "Central Park",
                        Description = "The most visited urban park in the United States"
                    }
                }
            },
            new()
            {
                Id = 2,
                Name = "Santa Cruz de la Sierra",
                Description = "City 2 description",
                PointsOfInterest = new List<PointOfInterestDto>
                {
                    new ()
                    {
                        Id =  2,
                        Name = "Ventura Mall",
                        Description = "Ventura Mall"
                    },
                    new ()
                    {
                        Id =  3,
                        Name = "Las Brisas",
                        Description = "Las Brisas"
                    },
                    new ()
                    {
                        Id =  4,
                        Name = "Plaza 24 de septiembre",
                        Description = "La plazanga"
                    }
                }
            },
            new()
            {
                Id = 3,
                Name = "Paris",
                Description = "City 3 description",
                PointsOfInterest = new List<PointOfInterestDto>
                {
                    new ()
                    {
                        Id =  5,
                        Name = "Eiffel Tower",
                        Description = "The most visited tower in France"
                    },
                    new ()
                    {
                        Id =  6,
                        Name = "Some french place",
                        Description = "I know nothing about France and its places"
                    },
                }
            }
        };
    }
}