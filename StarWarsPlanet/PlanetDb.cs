using Microsoft.EntityFrameworkCore;
using StarWarsPlanets;
using System.Xml;

namespace StarWarsPlanet
{
    public class PlanetDb : DbContext
    {
        public PlanetDb(DbContextOptions<PlanetDb> options)
            : base(options) { }

        public DbSet<FavoritePlanet> planetsDB => Set<FavoritePlanet>();
    }
}
