using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.Api.Entities;

public class City
{
    [Key]  // As the member has Id as name, it would automatically be the primary key
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Generation of Id
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }

    public ICollection<PointOfInterest> PointsOfInterest { get; set; } = new List<PointOfInterest>();

    public City(string name)
    {
        Name = name;
    }
}