using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Advert;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HCMS.Common.RoleConstants;

namespace HCMS.Web.Api.Controllers
{
    [Route("api/advert")]
    [ApiController]
    public class AdvertApiController : ControllerBase
    {
        private readonly IAdvertService advertService;

        public AdvertApiController(IAdvertService advertService)
        {
            this.advertService = advertService;
        }

        [HttpPost("add")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public async Task<IActionResult> Add([FromBody] AdvertAddDto model)
        {

            try
            {
               await this.advertService.AddAsync(model);
                return Ok("Job offer was successfully added!");
            } 
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
