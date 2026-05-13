
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PersonApi.Context;
using PersonApi.Repositories;
using PersonApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Test database"));

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Redirects HTTP requests to HTTPS. This is important for security, ensuring that all communication between the client and server is encrypted.
app.UseHttpsRedirection();

//endpoints for the application. In this case, it maps a GET request to the root URL ("/") to return a simple "Hello World!" response.
app.MapGet("/hello", () =>
{
    return "Hello World!";

});

app.MapControllers();



//runs the application and blocks the calling thread until host shutdown is triggered.
app.Run();


