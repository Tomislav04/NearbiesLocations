using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NearbiesLocations.Models
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }
        public string? Name { get; set; } // Naziv lokacije

        public string? Address { get; set; } // Adresa lokacije

        [Column(TypeName = "decimal(17, 15)")]
        public decimal? Latitude { get; set; } // Geografska širina

        [Column(TypeName = "decimal(17, 15)")]
        public decimal? Longitude { get; set; } // Geografska dužina
        public string? ExternalID { get; set; } // ID koji je dodijeljen od strane vanjskog API servisa (npr. Google Places ID)
        public ICollection<LocationCategory>? LocationCategories { get; set; } // Lista izvršenih pretraga
    }
}
