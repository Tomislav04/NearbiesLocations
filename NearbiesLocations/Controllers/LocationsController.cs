using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NearbiesLocations.Data;
using NearbiesLocations.Models;
using NearbiesLocations.Services.Interface;
using Newtonsoft.Json;
using System.Net.Http;

namespace NearbiesLocations.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [Authorize]
        [HttpPost("search")]
        public async Task<IActionResult> SearchLocations(decimal latitude, decimal longitude, string? category = null)
        {
            try
            {
                var locations = await _locationService.GetLocationsAsync(latitude, longitude, category);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("favourite")]
        public async Task<IActionResult> AddFavourite(int locationID)
        {
            try
            {
                _locationService.SaveFavouriteLocation(locationID);
                return Ok("Lokacija dodana u favorite!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}