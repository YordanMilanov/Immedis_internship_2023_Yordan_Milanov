using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Employee;
using HCMS.Services.ServiceModels.Recommendation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HCMS.Web.Api.Controllers
{
    [Route("api/recommendation")]
    [ApiController]
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
        [Authorize]
        public async Task<IActionResult> Add([FromBody]string recommendationModel)
        {
            RecommendationDto recommendationDto = JsonConvert.DeserializeObject<RecommendationDto>(recommendationModel, JsonSerializerSettingsProvider.GetCustomSettings())!;

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
    }
}
