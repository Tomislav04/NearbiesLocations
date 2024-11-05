using NearbiesLocations.Models;

namespace NearbiesLocations.Services.Interface
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetLocationsAsync(decimal lat, decimal lng, string? category = null);
        void SaveFavouriteLocation(int LocationID);
    }
}

