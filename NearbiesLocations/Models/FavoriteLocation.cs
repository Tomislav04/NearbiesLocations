using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NearbiesLocations.Models
{
    public class FavoriteLocation
    {
        [Key]
        public int FavoriteLocationID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; } // ID korisnika koji je dodao omiljenu lokaciju
        public User User { get; set; } // Navigacijska svojstvo prema korisniku

        [ForeignKey("Location")]
        public int LocationID { get; set; } // ID omiljene lokacije
        public Location Location { get; set; } // Navigacijska svojstvo prema lokaciji

        public DateTime AddedDate { get; set; } // Datum dodavanja u omiljene
    }
}
