using Newtonsoft.Json;

namespace NearbiesLocations.Models
{
    public class GeometryViewModel
    {
        [JsonProperty("location")]
        public GooglePlaceLocationViewModel Location { get; set; }
    }
}
