using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.Api.Entities;

public class PointOfInterest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }
    [ForeignKey("CityId")]  // Foreign key's name from navigation property in this class
    public City? City { get; set; }  // Navigation property (foreign key) by convention a relationship will be created by the ORM
    public int CityId { get; set; }

    public PointOfInterest(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return $"ID: {Id}\nName: {Name}\nDescription: {Description ?? "No description provided."}";
    }
}