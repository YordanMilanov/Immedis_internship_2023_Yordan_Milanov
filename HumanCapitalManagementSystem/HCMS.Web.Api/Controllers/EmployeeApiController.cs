using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels;
using HCMS.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("add")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> GetEmployeeDtoByUserId([FromBody] Guid userIdGuid)
        {
            EmployeeDto? employeeDto = await employeeService.GetEmployeeDtoByUserIdAsync(userIdGuid);

            if(employeeDto != null)
            {
                return Ok(employeeDto);
            }

            return NotFound("Employee Not Found!");
        }
    }
}
