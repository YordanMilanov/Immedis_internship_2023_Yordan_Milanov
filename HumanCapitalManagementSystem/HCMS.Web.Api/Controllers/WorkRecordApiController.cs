using HCMS.Services.ServiceModels;
using HCMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HCMS.Services.ServiceModels.WorkRecord;
using HCMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<IActionResult> WorkRecordAdd()
        {
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
            WorkRecordDto model = JsonConvert.DeserializeObject<WorkRecordDto>(jsonReceived)!;


            try
            {
                await workRecordService.AddWorkRecordAsync(model);
                return Ok("The information has been succssesfully updated!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("allByEmployeeId")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> WorkRecordAllByEmployeeId([FromQuery] Guid id)
        {
            try
            {
                List<WorkRecordDto> allWorkRecords = await workRecordService.GetAllWorkRecordsDtosByEmployeeIdAsync(id);
                string jsonString = JsonConvert.SerializeObject(allWorkRecords, JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(jsonString, "application/json");
            } catch (Exception)
            {
                return BadRequest("Could not complete get all work records operation!");
            }
        }

        [HttpGet("all")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize(Roles = "AGENT,ADMIN")]
        public async Task<IActionResult> AllWorkRecords()
        {
            try
            {
                List<WorkRecordDto> allWorkRecords = await workRecordService.GetAllWorkRecordsDtosAsync();
                string jsonString = JsonConvert.SerializeObject(allWorkRecords, JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(jsonString, "application/json");
            }
            catch (Exception)
            {
                return BadRequest("Could not complete get all work records operation!");
            }
        }

        [HttpPost("currentPage")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> WorkRecordsCurrentPage()
        {
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
            WorkRecordQueryDto model = JsonConvert.DeserializeObject<WorkRecordQueryDto>(jsonReceived)!;

            try
            {
                model.TotalWorkRecords = await workRecordService.GetWorkRecordsCountByEmployeeIdAsync(model.EmployeeId);

                //set current page models
                model.WorkRecords = await workRecordService.GetWorkRecordsPageAsync(model);


                string jsonString = JsonConvert.SerializeObject(model, JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(jsonString, "application/json");

            } catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to load the page!");
            }
        }
    }
}
