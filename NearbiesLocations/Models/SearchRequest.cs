using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NearbiesLocations.Models
{
    public class SearchRequest
    {
        [Key]
        public int SearchRequestID { get; set; }

        [Column(TypeName = "decimal(17, 15)")]
        public decimal? Latitude { get; set; } // Latituda pretraživane lokacije

        [Column(TypeName = "decimal(17, 15)")]
        public decimal? Longitude { get; set; } // Longituda pretraživane lokacije
        public DateTime SearchTime { get; set; } // Vrijeme kad je pretraga izvršena

        [ForeignKey("User")]
        public int UserID { get; set; } // ID korisnika koji je izvršio pretragu
        public User User { get; set; } // Navigacijska svojstvo prema korisniku

        [ForeignKey("Category")]
        public int? CategoryID { get; set; } // ID kategorije lokacije
        public Category Category { get; set; } // Navigacijska svojstvo prema kategoriji lokacije
        public ICollection<RequestResponse>? RequestResponse { get; set; } // Lista izvršenih pretraga
    }
}
