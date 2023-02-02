using CityInfo.Api;
using CityInfo.Api.DbContexts;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// builder.Logging.ClearProviders();  // There is nothing to log - overrides appSettings configuration
// builder.Logging.AddConsole();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // Define default input or output format
    // options.InputFormatters.Add();
    // options.OutputFormatters.Add();

    // To specify whether the response format is accepted or not
    // Example: Request XML response but it is not supported by the API
    options.ReturnHttpNotAcceptable = true;  
})
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();  // This line adds XML support to the API

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Adds singleton instance of FileExtensionContentTypeProvider to .NET Core service container.
// This class provides information about the content type (MIME) for a given file extension. 
// It is used to set the content type of an HTTP response based on the file extension of a file being served or downloaded.
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
#if DEBUG  // Compiler directive
builder.Services.AddTransient<IMailService, LocalMailService>();  // Transient lifetime services are created each time they are requested - lightweight, stateless services
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif
// builder.Services.AddScoped<>();     // Scoped lifetime services are created once per request
// builder.Services.AddSingleton<>();  // Singleton lifetime services are created the first time they are requested

builder.Services.AddSingleton<CitiesDataStore>();
builder.Services.AddDbContext<CityInfoContext>(
    optionsBuilder => optionsBuilder.UseSqlServer(
        builder.Configuration["ConnectionStrings:CityInfoDb"]));
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();  // Repositories are best fit with scoped
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline
// Middleware
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    app.MapControllers(); // Routing
});
app.Run();
