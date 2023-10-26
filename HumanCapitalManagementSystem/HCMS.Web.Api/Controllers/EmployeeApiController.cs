using HCMS.Common.JsonConverter;
using HCMS.Common.Structures;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels;
using HCMS.Services.ServiceModels.User;
using HCMS.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;

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
    }
}
