using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarWarsPlanet
{
    public class FavoritePlanet
    {
        
        [Key]
        public string name { get; set; }

        public string? rotation_period { get; set; }

        public string? orbital_period { get; set; }

        public string? diameter { get; set; }

        public string? climate { get; set; }

        public string? gravity { get; set; }

        public string? terrain { get; set; }

        public string? surface_water { get; set; }

        public string? population { get; set; }
        [NotMapped]
        public  List<String> residents { get; set;}
        [NotMapped]
        public List<String> films { get; set; }

        public DateTime? created { get; set; }

        public DateTime? edited { get; set; }
        public string? url { get; set; }
    }





}
