using AutoMapper;
using HCMS.Services;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Company;
using HCMS.Services.ServiceModels.Education;
using HCMS.Services.ServiceModels.User;
using HCMS.Services.ServiceModels.WorkRecord;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace HCMS.Web.Api.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyApiController : ControllerBase
    {
        private readonly ICompanyService companyService;

        public CompanyApiController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet("AllCompanies")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAllCompanyNames()
        {
            try
            {
                IEnumerable<string> names = await companyService.GetAllCompanyNamesAsync();
                if(!names.Any()) {
                    throw new Exception();
                }
                string jsonToSend = JsonConvert.SerializeObject(names, Formatting.Indented);

                return Content(jsonToSend, "application/json");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetCompanyDtoByEmployeeId")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCompanyDtoByEmployeeId([FromQuery]string employeeId)
        {
            try
            {
                CompanyDto companyDto = await companyService.GetCompanyDtoByEmployeeIdAsync(Guid.Parse(employeeId));
                string jsonToSend = JsonConvert.SerializeObject(companyDto, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(jsonToSend, "application/json");
            } catch (Exception)
            {
                return NotFound("Company not found!");
            }
        }

        [HttpPost("all")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAllCompanies([FromBody]QueryDto queryDto)
        {
            try
            {
                QueryDtoResult<CompanyDto> companyQueryDto = await companyService.GetCompaniesPageAndTotalCountAsync(queryDto);
                string jsonString = JsonConvert.SerializeObject(companyQueryDto, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(jsonString, "application/json");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to load the page!");
            }
        }


        [HttpGet("GetCompanyById")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCompanyDtoById([FromQuery] string id)
        {
            try {
            CompanyDto companyDto = await companyService.GetCompanyDtoByIdAsync(Guid.Parse(id));
            string jsonToSend = JsonConvert.SerializeObject(companyDto, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());
            return Content(jsonToSend, "application/json");
            } catch (Exception) {
                return NotFound("Company not found!");
            }

        }

        [HttpPost("updateCompany")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateCompany()
        {
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();

            CompanyDto model;
            try
            {
                // Deserialize the JSON
                model = JsonConvert.DeserializeObject<CompanyDto>(jsonReceived, JsonSerializerSettingsProvider.GetCustomSettings())!;
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON data");
            }

            try
            {
                if (model.Id == Guid.Empty)
                {
                    await companyService.AddCompanyDtoAsync(model);
                    return Content("Successfully added the company information!", "application/json");
                }
                else
                {
                    await companyService.EditCompanyDtoAsync(model);
                    return Content("Successfully updated the company information!", "application/json");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCompanyNameById")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCompanyNameById([FromQuery] string companyId)
        {
            try
            {
                string companyName = await companyService.GetCompanyNameById(Guid.Parse(companyId));
                return Content(companyName, "application/json");
            }
            catch (Exception)
            {
                return NotFound("Company not found!");
            }

        }
    }
}
