using HCMS.Services;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.User;
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
                IEnumerable<string> names = await companyService.GetAllCompanyNames();
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
    }
}
