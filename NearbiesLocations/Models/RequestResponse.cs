using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NearbiesLocations.Models
{
    public class RequestResponse
    {
        [Key]
        public int RequestResponseID { get; set; }

        [ForeignKey("SearchRequest")]
        public int SearchRequestID { get; set; }
        public SearchRequest SearchRequest { get; set; } 
        public string LocationsIDs { get; set; }
    }
}
