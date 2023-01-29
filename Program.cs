var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // Define default input or output format
    // options.InputFormatters.Add();
    // options.OutputFormatters.Add();

    // To specify whether te response format is accepted or not
    // Example: Request XML response but it is not supported by the API
    options.ReturnHttpNotAcceptable = true;  
}).AddXmlDataContractSerializerFormatters();  // This line adds XML support to the API

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
