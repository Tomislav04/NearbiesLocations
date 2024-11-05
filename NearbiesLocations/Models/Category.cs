using System.ComponentModel.DataAnnotations;

namespace NearbiesLocations.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [MaxLength(500)]
        public string Name { get; set; } // Naziv kategorije
        public ICollection<SearchRequest>? SearchRequests { get; set; } // Lista izvršenih pretraga
        public ICollection<LocationCategory>? LocationCategories { get; set; } // Lista izvršenih pretraga
    }
}
