using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NearbiesLocations.Models
{
    public class LocationCategory
    {
        [Key]
        public int LocationCategoryID { get; set; }

        [ForeignKey("Location")]
        public int LocationID { get; set; } // ID omiljene lokacije
        public Location Location { get; set; } // Navigacijska svojstvo prema lokaciji

        [ForeignKey("Category")]
        public int CategoryID { get; set; } // ID kategorije lokacije
        public Category Category { get; set; } // Navigacijska svojstvo prema kategoriji lokacije
    }
}
