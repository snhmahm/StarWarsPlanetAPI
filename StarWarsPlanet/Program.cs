using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using StarWarsPlanet;
using StarWarsPlanets;
using System.Net;
using System.Numerics;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PlanetDb>(opt => opt.UseInMemoryDatabase("Planets"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

var starWarPlanetAPI = app.MapGroup("/planets");
starWarPlanetAPI.MapGet("/allPlanets", GetPlanet);
starWarPlanetAPI.MapPost("/addFavorite", CreateFavorite);
starWarPlanetAPI.MapGet("/all", GetCompleteList);
starWarPlanetAPI.MapDelete("/{name}", DeletePlanet);
starWarPlanetAPI.MapGet("/random", GetRandomPlanet);

//Get Random planet from SWAPI which is not present in DB
static async Task<IResult> GetRandomPlanet(PlanetDb db)
{
    HttpClient client = new HttpClient();
    HttpResponseMessage response = await client.GetAsync("https://swapi.dev/api/planets/");
    Planets? planet = new Planets();
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Hello Random SWAPI Planet!");
        var planetString = await response.Content.ReadAsStringAsync();
        planet = JsonSerializer.Deserialize<Planets>(planetString);
    
    foreach(Result i in planet.results)
    {
            var result = await db.planetsDB.FindAsync(i.name);
            if (result is null) return TypedResults.Ok(i);
        }
   }

return TypedResults.Ok(planet);
}

//GET ALL Favorite planets
static async Task<IResult> GetCompleteList(PlanetDb db)
{
    return TypedResults.Ok(await db.planetsDB.ToListAsync());
}


//ADD favorite Flat
static async Task<IResult> CreateFavorite(FavoritePlanet planets, PlanetDb db)
{   
  
        db.planetsDB.Add(planets);
  
    await db.SaveChangesAsync();

    return TypedResults.Created($"/favorites/{planets.name}", planets);
}


//GET ALL planets from SWAPI API
static async Task<IResult> GetPlanet()
    {

     HttpClient client = new HttpClient();
    Planets? planet = new Planets();
      HttpResponseMessage response = await client.GetAsync("https://swapi.dev/api/planets/");
   
    if (response.IsSuccessStatusCode)
        {
        Console.WriteLine("Hello SWAPI!");
        var planetString = await response.Content.ReadAsStringAsync();
       planet =JsonSerializer.Deserialize<Planets>(planetString);


    }
         return TypedResults.Ok(planet);
    }

//Delete  Favorite Planet
static async Task<IResult> DeletePlanet(string name, PlanetDb db)
{
    if (await db.planetsDB.FindAsync(name) is FavoritePlanet planet)
    {
        db.planetsDB.Remove(planet);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}


app.Run();
