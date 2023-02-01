using CityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.DbContexts;

public class CityInfoContext : DbContext
{
    // Deriving from DbContext ensures that these properties are initialized
    // as non-null after leaving the base constructor. So the warning about
    // nullable value can be ignored
    public DbSet<City> Cities { get; set; } = null!;  // null-forgiving operator
    public DbSet<PointOfInterest> PointOfInterests { get; set; } = null!;
    // Basically, null! applies the ! operator to the value null.
    // This overrides the nullability of the value null to non-nullable,
    // telling the compiler that null is a "non null" type.

    public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
    {
        
    }
    
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer("connectionstring");
    //     base.OnConfiguring(optionsBuilder);
    // }
    
    
}