using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCMS.Web.Api.Controllers
{

    [Route("api/application")]
    [ApiController]
    [Authorize]
    public class ApplicationApiController : Controller
    {

        private readonly IApplicationService applicationService;
        public ApplicationApiController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }


        [HttpPost("apply")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IActionResult> Apply([FromBody] ApplicationDto applicationDto)
        {
            try
            {
                await this.applicationService.AddAsync(applicationDto);
                return Ok("You have successfully applied for the job");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
