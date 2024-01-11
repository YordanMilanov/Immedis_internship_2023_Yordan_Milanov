using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Education;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HCMS.Web.Api.Controllers
{
    [Route("api/education")]
    [ApiController]
    public class EducationApiController : ControllerBase
    {

        private readonly IEducationService educationService;

        public EducationApiController(IEducationService educationService)
        {
            this.educationService = educationService;
        }

        [HttpPost("EditEducation")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> EditEducation()
        {
            // Read the JSON from request body
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();

            EducationDto model;
            try
            {
                // Deserialize the JSON
                model = JsonConvert.DeserializeObject<EducationDto>(jsonReceived, JsonSerializerSettingsProvider.GetCustomSettings())!;
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON data");
            }

            try
            {
                if (model.Id == Guid.Empty)
                {
                    try
                    {
                        await educationService.AddEducationAsync(model);
                        return Content("Successfully added the education record!", "application/json");
                    }
                    catch (Exception)
                    {
                        return BadRequest("Unexpected error occurred!");
                    }
                }
                else
                {
                    await educationService.EditEducationAsync(model);
                    return Content("Successfully updated the education record!", "application/json");

                }
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred!");
            }
        }

        [HttpGet("EducationsPageByEmployeeId")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> GetEducationsPageByEmployeeId([FromQuery] string employeeId, [FromQuery] int page)
        {
            try
            {
                List<EducationDto> educations = await educationService.GetEducationPageDtosByEmployeeIdAsync(Guid.Parse(employeeId), page);

                string educationsJson = JsonConvert.SerializeObject(educations, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());

                return Content(educationsJson, "application/json");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to load your educations!");
            }
        }


        [HttpGet("EducationsCountByEmployeeId")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> GetEducationsCountByEmployeeId([FromQuery] string employeeId)
        {
            try
            {
                int count = await educationService.GetEducationCountByEmployeeIdAsync(Guid.Parse(employeeId));
                string educationsCountJson = JsonConvert.SerializeObject(count, Formatting.Indented);

                return Content(educationsCountJson, "application/json");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to load your educations count!");
            }
        }

        [HttpGet("EducationsDtoById")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> GetEducationDtoById([FromQuery] string educationId)
        {
            try
            {
                EducationDto educationDto = await educationService.GetEducationDtoByIdAsync(Guid.Parse(educationId));
                string educationDtoJson = JsonConvert.SerializeObject(educationDto, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(educationDtoJson, "application/json");
            }
            catch (Exception)
            {
                return BadRequest("Education was not found!");
            }


        }

        [HttpDelete("DeleteById")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> DeleteById([FromQuery] string EducationId)
        {
            try
            {
                await educationService.DeleteById(Guid.Parse(EducationId));
                return Ok("Successfully deleted the education record!");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to delete your education information!");
            }
        }
    }
}
