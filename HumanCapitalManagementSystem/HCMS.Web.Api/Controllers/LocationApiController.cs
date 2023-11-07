using HCMS.Services.ServiceModels.Company;
using HCMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Location;

namespace HCMS.Web.Api.Controllers
{
    [Route("api/Location")]
    [ApiController]
    public class LocationApiController : ControllerBase
    {
        private readonly ILocationService locationService;

        public LocationApiController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        [HttpGet("GetLocationById")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetLocationById([FromQuery] string id)
        {
            try
            {
                LocationDto locationDto = await locationService.GetLocationDtoByIdAsync(Guid.Parse(id));
                string jsonToSend = JsonConvert.SerializeObject(locationDto, Formatting.Indented);
                return Content(jsonToSend, "application/json");
            }
            catch (Exception)
            {
                return NotFound("Location not found!");
            }
        }
    }
}
