using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Advert;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static HCMS.Common.RoleConstants;

namespace HCMS.Web.Api.Controllers
{
    [Route("api/advert")]
    [ApiController]
    public class AdvertApiController : ControllerBase
    {
        private readonly IAdvertService advertService;

        public AdvertApiController(IAdvertService advertService)
        {
            this.advertService = advertService;
        }

        [HttpPost("add")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public async Task<IActionResult> Add()
        {
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
            AdvertAddDto model = JsonConvert.DeserializeObject<AdvertAddDto>(jsonReceived, JsonSerializerSettingsProvider.GetCustomSettings())!;

            try
            {
                await this.advertService.AddAsync(model);
                return Ok("Job offer was successfully added!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("all")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromBody] AdvertQueryDto advertQueryDto)
        {
            try
            {
                AdvertQueryDtoResult queryResult = await this.advertService.GetCurrentPageAsync(advertQueryDto);
                string jsonToSend = JsonConvert.SerializeObject(queryResult, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(jsonToSend, "application/json");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred!");
            }
        }
    }
}
