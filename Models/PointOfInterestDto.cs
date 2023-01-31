namespace CityInfo.Api.Models;

public class PointOfInterestDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}\nName: {Name}\nDescription: {Description ?? "No description provided."}";
    }
}