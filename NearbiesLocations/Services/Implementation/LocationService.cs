using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NearbiesLocations.Data;
using NearbiesLocations.Models;
using NearbiesLocations.Models.ViewModels;
using NearbiesLocations.Services.Interface;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace NearbiesLocations.Services.Implementation
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;
        private readonly LocationContext _context;
        private readonly IHubContext<LocationHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocationService(HttpClient httpClient, 
                               IConfiguration configuration, 
                               LocationContext context, 
                               IHubContext<LocationHub> hubContext,
                               IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _context = context;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(decimal lat, decimal lng, string? category = null)
        {
            var apiKey = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.AuthenticationInstant)?.Value;
            var url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={lat},{lng}&radius=1500&type={category}&key={apiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var results = await response.Content.ReadAsStringAsync();
            var placesResponse = JsonConvert.DeserializeObject<GooglePlacesResponseViewModel>(results);
            IList<PlaceResponseViewModel>? data = placesResponse.Results;

            if (data == null || !data.Any())
            {
                return Enumerable.Empty<Location>();
            }

            var placeIds = data.Select(d => d.ExternalID).ToList();

            if (data.Count > 0 && category != null)
            {
                var categoryExists = _context.Categories.Any(x => x.Name == category);
                if (!categoryExists)
                {
                    var addCategory = new Category() { Name = category };
                    _context.Categories.Add(addCategory);
                    await _context.SaveChangesAsync();
                }
            }

            foreach (var location in data)
            {
                var exists = await _context.Locations.AnyAsync(l => l.ExternalID == location.ExternalID);

                if (!exists)
                {
                    var newLocation = new Location
                    {
                        Name = location.Name,
                        Address = location.Address,
                        Latitude = location.Geometry?.Location?.Latitude,
                        Longitude = location.Geometry?.Location?.Longitude,
                        ExternalID = location.ExternalID
                    };
                    _context.Locations.Add(newLocation);
                    await _context.SaveChangesAsync();
                }

                foreach (var categoryName in location.Categories)
                {
                    var categoryTempID = _context.Categories.Where(x => x.Name == categoryName)?.FirstOrDefault()?.CategoryID ?? 0;
                    if(categoryTempID > 0)
                    {
                        var locationCategoryexists = await _context.LocationCategory.AnyAsync(l => l.Location.ExternalID == location.ExternalID && l.CategoryID == categoryTempID);
                        if (!locationCategoryexists)
                        {
                            var locationID = _context.Locations.Where(x => x.ExternalID == location.ExternalID)?.FirstOrDefault()?.LocationID ?? throw new Exception("Lokacija ne postoji u sustavu.");
                            var newLocationCategory = new LocationCategory
                            {
                                LocationID = locationID,
                                CategoryID = categoryTempID
                            };
                            _context.LocationCategory.Add(newLocationCategory);
                            await _context.SaveChangesAsync();
                        }

                    }
                }                             
            }

            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username) ?? throw new Exception("Korisnik ne postoji u sustavu.");
            var searchCategory = _context.Categories.Where(x => x.Name == category).FirstOrDefault(); ;

            var searchRequest = new SearchRequest
            {
                Latitude = lat,
                Longitude = lng,
                CategoryID = searchCategory?.CategoryID,
                SearchTime = DateTime.Now,
                UserID = user?.UserID ?? 0
            };

            _context.SearchRequests.Add(searchRequest);
            await _context.SaveChangesAsync();

            var matchingLocations = await _context.Locations
                .Where(location => placeIds.Contains(location.ExternalID))
                .ToListAsync();

            var locationsIds = String.Join(", ", matchingLocations.Select(x => x.LocationID));

            var requestReponse = new RequestResponse()
            {
                SearchRequestID = searchRequest.SearchRequestID,
                LocationsIDs = locationsIds
            };

            _context.RequestResponses.Add(requestReponse);
            await _context.SaveChangesAsync();


            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"\nUserID: {user.UserID}\nUsername: {user.Username}\nLat: {lat}\nLng: {lng}\nCategory: {category}");

            return matchingLocations;
        }

        public void SaveFavouriteLocation(int locationID)
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var user =  _context.Users.FirstOrDefault(u => u.Username == username) ?? throw new Exception("Korisnik ne postoji u sustavu.");

            var location = _context.Locations.Where(l => l.LocationID == locationID).FirstOrDefault() ?? throw new Exception("Lokacija s tim external ID ne postoji u sustavu.");

            var existingFavouriteLocation = _context.FavoriteLocations.Any(f => f.LocationID == location.LocationID && f.UserID == user.UserID);
            if (existingFavouriteLocation)
            {
                throw new Exception("Lokacija već postoji u favoritima."); ;
            }

            var favouriteLocation = new FavoriteLocation()
            {
                UserID = user.UserID,
                LocationID = location.LocationID,
                AddedDate = DateTime.Now
            };

            _context.FavoriteLocations.Add(favouriteLocation);
            _context.SaveChanges();
        }







    }
    
}
