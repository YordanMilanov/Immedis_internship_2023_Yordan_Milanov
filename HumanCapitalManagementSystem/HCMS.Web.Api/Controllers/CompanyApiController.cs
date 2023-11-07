using AutoMapper;
using HCMS.Services;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Company;
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
                string jsonToSend = JsonConvert.SerializeObject(companyDto, Formatting.Indented);
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
        public async Task<IActionResult> GetAllCompanies()
        {
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
            CompanyQueryDto model = JsonConvert.DeserializeObject<CompanyQueryDto>(jsonReceived)!;

            try
            {
                object result = await companyService.GetCompaniesPageAndTotalCountAsync(model);
                if (result is (int totalCount, List<CompanyDto> companies))
                {
                    model.Companies = companies;
                    model.TotalCompanies = totalCount;

                    string jsonString = JsonConvert.SerializeObject(model, JsonSerializerSettingsProvider.GetCustomSettings());
                    return Content(jsonString, "application/json");
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to load the page!");
            }
        }

    }
}
