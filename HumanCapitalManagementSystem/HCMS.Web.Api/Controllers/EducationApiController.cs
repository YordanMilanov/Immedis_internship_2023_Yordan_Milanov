using AutoMapper;
using HCMS.Services;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Education;
using HCMS.Services.ServiceModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HCMS.Web.Api.Controllers
{
    [Route("api/education")]
    [ApiController]
    public class EducationApiController : ControllerBase
    {

        private readonly IEducationService educationService;
        private readonly IMapper mapper;

        public EducationApiController(IEducationService educationService, IMapper mapper)
        {
            this.educationService = educationService;
            this.mapper = mapper;
        }

        [HttpPost("EditEducation")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
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
                        return Content("Succssesfully added the education record!", "application/json");
                    }
                    catch (Exception)
                    {
                       return BadRequest("Unexpected error occurred!");
                    }
                } 
                else
                {
                    await educationService.EditEducationAsync(model);
                    return Content("Succssesfully updated the education record!", "application/json");

                }
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred!");
            }
        }
    }
}
