using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using NearbiesLocations.Data;
using NearbiesLocations.Helpers;
using NearbiesLocations.Models;
using NearbiesLocations.Services.Implementation;
using NearbiesLocations.Services.Interface;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

// Add services to the container.
builder.Services.AddAuthentication("BasicAuthentication")
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.MaxDepth = 50;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor(); // Registracija IHttpContextAccessor-a
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddDbContext<LocationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient<ILocationService, LocationService>();
builder.Services.AddSignalR();


// Dodavanje Swagger servisa
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Omogućavanje Swagger-a
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.Use(async (context, next) =>
//{
//    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
//    if (authHeader != null && authHeader.StartsWith("Basic "))
//    {
//        // Dekodiranje i provjera API ključa
//    }
//    else
//    {
//        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//        return;
//    }
//    await next.Invoke();
//});

app.MapHub<LocationHub>("/locationHub");

app.UseHttpsRedirection();

app.UseMiddleware<CustomAuthenticationHandler>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
