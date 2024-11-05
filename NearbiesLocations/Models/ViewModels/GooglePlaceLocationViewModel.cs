using Newtonsoft.Json;

namespace NearbiesLocations.Models
{
    public class GooglePlaceLocationViewModel
    {
        [JsonProperty("lat")]
        public decimal? Latitude { get; set; }

        [JsonProperty("lng")]
        public decimal? Longitude { get; set; }
    }
}
