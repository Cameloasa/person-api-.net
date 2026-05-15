
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonApi.Context;
using PersonApi.Models.DTOs;
using PersonApi.Models.Requests;
using PersonApi.Repositories;
using PersonApi.Services;
using PersonApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// generate documentation
builder.Services.AddOpenApi();

//Api explorer for swagger -UI
builder.Services.AddEndpointsApiExplorer();

//context add in memory db
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseInMemoryDatabase("Test database"));
builder.Services.AddDbContext<IdentityContext>(options => 
    options.UseInMemoryDatabase("Test database"));

//config for identity 
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options => 
{ 
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<IdentityContext>();
    
//login
builder.Services.AddAuthorization();

// Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = false;
    });

//services
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

//mappers
builder.Services.AddAutoMapper(options => options.AddMaps(typeof(Program)));

//Validators
builder.Services.AddScoped<IValidator<CreatePersonRequest>, CreatePersonRequestValidator>();
builder.Services.AddScoped<IValidator<AdressDTO>, AdressDTOValidator>();

var app = builder.Build();

//variable for scope
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
    identityContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Redirects HTTP requests to HTTPS. This is important for security, ensuring that all communication between the client and server is encrypted.
app.UseHttpsRedirection();

// Identity endpoints
//add autentication auth
app.UseAuthentication();
app.UseAuthorization();

//use Identity user
app.MapIdentityApi<IdentityUser>();
app.MapControllers();

//endpoints for the application. In this case, it maps a GET request to the root URL ("/") to return a simple "Hello World!" response.
app.MapGet("/hello", () =>
{
    return "Hello World!";

});

//runs the application and blocks the calling thread until host shutdown is triggered.
app.Run();


