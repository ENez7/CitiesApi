using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]  // Base link for this controller
public class CitiesController : ControllerBase
{
    [HttpGet]
    public JsonResult GetCities()
    {
        return new JsonResult(
            new List<object>
            {
                new { id = 1, Name = "New York City" },
                new { id = 2, Name = "Santa Cruz de la Sierra" }
            }
        );
    }
}