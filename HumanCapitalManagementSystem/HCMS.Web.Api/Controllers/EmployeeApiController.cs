using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Company;
using HCMS.Services.ServiceModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HCMS.Web.Api.Controllers
{


    [Route("api/employee")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeeApiController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet("GetEmployeeDtoByUserId")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> GetEmployeeDtoByUserId([FromQuery] string userId)
        {
            //get the employeeDto
            EmployeeDto? employeeDto = await employeeService.GetEmployeeDtoByUserIdAsync(Guid.Parse(userId));

           //if employee found
            if (employeeDto != null)
            {

                //convert to json (formatting.indented -> beautifying for better human reading)
                string json = JsonConvert.SerializeObject(employeeDto, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());

                //by default content sets the http status code to 200
                return Content(json, "application/json");
            }
            //if employee not found
            return NotFound("Employee Not Found!");
        }

        [HttpPost("UpdateEmployee")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee()
        {
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
            EmployeeDto model = JsonConvert.DeserializeObject<EmployeeDto>(jsonReceived, JsonSerializerSettingsProvider.GetCustomSettings())!;


            try
            {
                await employeeService.UpdateEmployeeAsync(model);
                return Ok("The information has been succssesfully updated!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("EmployeeIdByUserId")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> GetEmployeeIdByUserId([FromQuery] string userId)
        {

            try
            {
                Guid? employeeId = await employeeService.GetEmployeeIdByUserId(Guid.Parse(userId));

                return Content(employeeId.ToString()!, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateEmployeeCompany")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> UpdateEmployeeCompany()
        {
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
            EmployeeCompanyUpdateDto model = JsonConvert.DeserializeObject<EmployeeCompanyUpdateDto>(jsonReceived)!;
            try
            {
                await employeeService.UpdateEmployeeCompanyByCompanyName(model.Id, model.CompanyName);
                return Ok("The information has been succssesfully updated!");

            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occured while trying to update to new company!");
            }
        }

        [HttpPost("page")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize(Roles = "AGENT,ADMIN")]
        public async Task<IActionResult> GetEmployeeCurrentPage()
        {

            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
            QueryDto model = JsonConvert.DeserializeObject<QueryDto>(jsonReceived)!;
            try
            {
                QueryDtoResult<EmployeeDto> employeeQueryDto = await employeeService.GetCurrentPageAsync(model);
                string jsonToSend = JsonConvert.SerializeObject(employeeQueryDto, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(jsonToSend, "application/json");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occured while trying to update to new company!");
            }
        }

        [HttpGet("Dismiss")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> EmployeeDismissCompany([FromQuery]string id)
        {
            try
            {
                await this.employeeService.RemoveEmployeeCompanyByIdAsync(Guid.Parse(id));
                return Ok("The company was successfully left!");
            } catch(Exception)
            {
                return BadRequest("Unexpected error occurred!");
            }
        }
    }
}
