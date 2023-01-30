using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

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
