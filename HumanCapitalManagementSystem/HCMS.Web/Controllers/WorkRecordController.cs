using AutoMapper;
using Castle.Core.Internal;
using HCMS.Services.ServiceModels.Employee;
using HCMS.Services.ServiceModels.WorkRecord;
using HCMS.Web.ViewModels.WorkRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
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
        [Authorize(Roles = "EMPLOYEE")]
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
        [Authorize(Roles = "EMPLOYEE")]
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
        public async Task<IActionResult> All()
        {
            //for ADMIN and AGENT
            if (HttpContext.User.Claims.Any(c => c.Type == "Role" && (c.Value == "ADMIN" || c.Value == "AGENT")))
            {
                string apiAllUrl = $"/api/workRecord/all";
                HttpResponseMessage responseAll = await httpClient.GetAsync(apiAllUrl);

                if (responseAll.IsSuccessStatusCode)
                {
                    string jsonContent = await responseAll.Content.ReadAsStringAsync();
                    List<WorkRecordDto> workRecords = JsonConvert.DeserializeObject<List<WorkRecordDto>>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings())!;
                    List<WorkRecordViewModel> workRecordViewModels = workRecords.Select(wr => mapper.Map<WorkRecordViewModel>(wr)).ToList();

                    if (workRecordViewModels.IsNullOrEmpty())
                    {
                        return View(new List<WorkRecordViewModel>());
                    }
                    return View(workRecordViewModels);
                }
                //for EMPLOYEE
                else if (HttpContext.User.Claims.Any(c => c.Type == "Role" && c.Value == "EMPLOYEE"))
                {
                    Guid employeeId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")!.Value);

                    if (employeeId == Guid.Empty)
                    {
                        return RedirectToAction("Add", "WorkRecord", new { redirect = "You have no personal information. Please first add personal information!" });
                    }

                    string apiEmployeeUrl = $"/api/workRecord/allByEmployeeId?employeeId={employeeId}";
                    HttpResponseMessage response = await httpClient.GetAsync(apiEmployeeUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        List<WorkRecordDto> employeeWorkRecords = JsonConvert.DeserializeObject<List<WorkRecordDto>>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings())!;

                        if (employeeWorkRecords.IsNullOrEmpty())
                        {
                            return RedirectToAction("Add", "WorkRecord", new { redirect = "You have no work records. Please first add work records!" });
                        }

                        return View(employeeWorkRecords);
                    }
                }
            }
            return View(new List<WorkRecordDto>());
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> All(WorkRecordQueryModel model)
        {

            return View(model);
        }




        [Authorize]
        public IActionResult Edit()
        {
            return View("Add");
        }
    }
}
