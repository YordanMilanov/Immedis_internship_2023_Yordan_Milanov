using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels;
using HCMS.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> GetEmployeeDtoByUserId([FromQuery] string userId)
        {
            EmployeeDto? employeeDto = await employeeService.GetEmployeeDtoByUserIdAsync(Guid.Parse(userId));
           
            if (employeeDto != null)
            {
                return Ok(employeeDto);
            }

            return NotFound("Employee Not Found!");
        }
    }
}
