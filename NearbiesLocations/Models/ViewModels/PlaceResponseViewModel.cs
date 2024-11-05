using Newtonsoft.Json;

namespace NearbiesLocations.Models.ViewModels
{
    public class PlaceResponseViewModel
    {

        [JsonProperty("place_id")]
        public string? ExternalID { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("vicinity")]
        public string? Address { get; set; }

        [JsonProperty("types")]
        public List<string> Categories { get; set; }

        [JsonProperty("geometry")]
        public GeometryViewModel Geometry { get; set; }
    }
}
