﻿using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.WorkRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            }
            catch (Exception)
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
            var requestBody = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            QueryDto queryDto = JsonConvert.DeserializeObject<QueryDto>(requestBody)!;
            string employeeId = HttpContext.Request.Query["employeeId"].ToString();
            try
            {
                QueryDtoResult<WorkRecordDto> result = await workRecordService.GetWorkRecordsPageAndTotalCountAsync(queryDto, Guid.Parse(employeeId));

                string jsonString = JsonConvert.SerializeObject(result, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(jsonString, "application/json");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to load the page!");
            }
        }

        [HttpDelete("DeleteById")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> DeleteById([FromQuery] string id)
        {
            try
            {
                await workRecordService.DeleteById(Guid.Parse(id));
                return Ok("Successfully deleted the work record!");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to delete your work record!");
            }
        }
    }
}
