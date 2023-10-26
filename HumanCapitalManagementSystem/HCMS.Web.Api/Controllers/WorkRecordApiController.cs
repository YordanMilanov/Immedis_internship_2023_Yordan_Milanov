using HCMS.Services.ServiceModels;
using HCMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HCMS.Services.ServiceModels.WorkRecord;
using HCMS.Services.Interfaces;

namespace HCMS.Web.Api.Controllers
{
    [Route("api/workRecord")]
    [ApiController]
    public class WorkRecordApiController : ControllerBase
    {
        private readonly IWorkRecordService workRecordService;

        public WorkRecordApiController(IWorkRecordService workRecordService)
        {
            this.workRecordService = workRecordService;
        }

        [HttpPost("add")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> WorkRecordAdd()
        {
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
            WorkRecordDto model = JsonConvert.DeserializeObject<WorkRecordDto>(jsonReceived)!;


            try
            {
                await workRecordService.AddWorkRecord(model);
                return Ok("The information has been succssesfully updated!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
