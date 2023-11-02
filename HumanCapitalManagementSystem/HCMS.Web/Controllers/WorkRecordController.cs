using AutoMapper;
using HCMS.Services.ServiceModels.WorkRecord;
using HCMS.Web.ViewModels.WorkRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using static HCMS.Common.NotificationMessagesConstants;

namespace HCMS.Web.Controllers
{
    public class WorkRecordController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public WorkRecordController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            httpClient = httpClientFactory.CreateClient("WebApi");
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add(string? redirect)
        {
            //check redirect
            if(redirect != null)
            {
                TempData[WarningMessage] = redirect;
                return View();
            }

            //if the user has no employee information redirect to add employee information
            if (!HttpContext.User.Claims.Any(c => c.Type == "EmployeeId")) {
                return RedirectToAction("Edit", "Employee", new { redirect = "Please first add your employee information to be able to add work records!" });
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(WorkRecordFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            WorkRecordDto workRecordDto = mapper.Map<WorkRecordDto>(model);
            workRecordDto.EmployeeId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")!.Value);

            string url = "api/workRecord/add";
            string json = JsonConvert.SerializeObject(workRecordDto);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.PostAsync(url, content);
           
            if(response.IsSuccessStatusCode)
            {
                string successMessage = "The work record has been successfully added!";
                TempData[SuccessMessage] = successMessage;
                return View();
            } else
            {
                ModelState.AddModelError("ErrorMessage", "Unexpected error occurred!");
                return View();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AllPersonal(WorkRecordQueryModel model)
        {
            if (model == null)
            {
                model = new WorkRecordQueryModel();
            }

            

            if (!HttpContext.User.Claims.Any(c => c.Type == "EmployeeId"))
            {
                return RedirectToAction("Edit", "Employee", new { redirect = "You have no personal information. Please first add personal information!" });
            }

            //For Employees
            if (HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "EMPLOYEE"))
            {
                WorkRecordQueryDto workRecordQueryDto = mapper.Map<WorkRecordQueryDto>(model);

                string EmployeeId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")!.Value;
                workRecordQueryDto.EmployeeId = Guid.Parse(EmployeeId);

                string url = "api/workRecord/currentPage";
                string json = JsonConvert.SerializeObject(workRecordQueryDto);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                //set JWT
                string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    WorkRecordQueryDto responseQueryDto = JsonConvert.DeserializeObject<WorkRecordQueryDto>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings())!;
                    WorkRecordQueryModel workRecordQueryModel = mapper.Map<WorkRecordQueryModel>(responseQueryDto);
                    ViewData["username"] = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Username")!.Value;
                    return View("All", workRecordQueryModel);
                }
                else
                {
                    return RedirectToAction("Add", "WorkRecord", new { redirect = "You have no personal information. Please first add personal information!" });
                }
            } 
            else
            {
                return RedirectToAction("Add", "WorkRecord", new { redirect = "To be done for AGENT and ADMIN" });
            }
        }
    }
}
