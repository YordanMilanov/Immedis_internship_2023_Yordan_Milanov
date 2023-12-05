using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Application;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static HCMS.Common.RoleConstants;

namespace HCMS.Web.Api.Controllers
{

    [Route("api/application")]
    [ApiController]
    [Authorize]
    public class ApplicationApiController : Controller
    {

        private readonly IApplicationService applicationService;
        public ApplicationApiController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }


        [HttpPost("apply")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IActionResult> Apply()
        {
            try
            {
                string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
                ApplicationDto model = JsonConvert.DeserializeObject<ApplicationDto>(jsonReceived)!;
                await this.applicationService.AddAsync(model);
                return Ok("You have successfully applied for the job");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("all")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(Roles =$"{AGENT},{ADMIN}")]
        public async Task<IActionResult> AllByAdvert([FromQuery]Guid advertId)
        {
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
            QueryDto model = JsonConvert.DeserializeObject<QueryDto>(jsonReceived)!;
            try
            {
                QueryDtoResult<ApplicationDto> applicationQueryDto = await applicationService.GetCurrentPageByAdvertAsync(model, advertId);
                string jsonToSend = JsonConvert.SerializeObject(applicationQueryDto, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(jsonToSend, "application/json");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to load the current page!");
            }
        }

        [HttpGet("accept")]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(200, Type = typeof(string))]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public async Task<IActionResult> Accept([FromQuery] Guid id)
        {
            try
            {
                await this.applicationService.acceptApplicationByIdAsync(id);
                return Ok("The employee was successfully recruited to the company!");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to accept the application!");
            }
        }

        [HttpDelete("decline")]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(200, Type = typeof(string))]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public async Task<IActionResult> Decline([FromQuery] Guid id)
        {
            try
            {
                await this.applicationService.acceptApplicationByIdAsync(id);
                return Ok("The application was successfully rejected!");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to decline the application!");
            }
        }
    }
}
