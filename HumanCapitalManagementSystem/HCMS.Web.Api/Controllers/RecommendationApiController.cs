using HCMS.Common;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Education;
using HCMS.Services.ServiceModels.Employee;
using HCMS.Services.ServiceModels.Recommendation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HCMS.Web.Api.Controllers
{
    [Route("api/recommendation")]
    [ApiController]
    [Authorize(Roles = $"{RoleConstants.AGENT},{RoleConstants.ADMIN}")]

    public class RecommendationApiController : ControllerBase
    {
        private readonly IRecommendationService recommendationService;

        public RecommendationApiController(IRecommendationService recommendationService)
        {
            this.recommendationService = recommendationService;
        }

        [HttpPost("add")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Add()
        {
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
            RecommendationDto recommendationDto = JsonConvert.DeserializeObject<RecommendationDto>(jsonReceived, JsonSerializerSettingsProvider.GetCustomSettings())!;

            try
            {
                await recommendationService.AddAsync(recommendationDto);
                return Ok("The recommendation was successfully added!");
            } 
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("page")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Page([FromBody]QueryDto queryDto, [FromQuery]Guid companyId)
        {
            try
            {
                QueryDtoResult<RecommendationDto> recommendationQueryDto = await recommendationService.GetCurrentPageAsync(queryDto, companyId);
                string jsonToSend = JsonConvert.SerializeObject(recommendationQueryDto, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(jsonToSend, "application/json");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to load the current page!");
            }
        }
    }
}
