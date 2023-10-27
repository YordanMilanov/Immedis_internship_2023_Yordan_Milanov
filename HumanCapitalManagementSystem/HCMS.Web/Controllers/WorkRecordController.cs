using AutoMapper;
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
        public async Task<IActionResult> Add()
        {
           IEnumerable<string> allCompanyNames = await GetAllCompanyNamesFromApiAsync();
            ViewData["allCompanyNames"] = allCompanyNames;
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
                IEnumerable<string> allCompanyNames = await GetAllCompanyNamesFromApiAsync();
                return RedirectToAction("Add");
            } else
            {
                ModelState.AddModelError("ErrorMessage", "Unexpected error occurred!");
                return View();
            }
        }


        [Authorize]
        public IActionResult All()
        {
            WorkRecordAllQueryModel model = new WorkRecordAllQueryModel();
            return View(model);
        }

        [Authorize]
        public IActionResult Edit()
        {
            return View("Add");
        }

        private async Task<IEnumerable<string>> GetAllCompanyNamesFromApiAsync()
        {
            //all companies select
            string allCompanyUrl = "api/company/AllCompanies";
            HttpResponseMessage response = await httpClient.GetAsync(allCompanyUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                IEnumerable<string> companies = JsonConvert.DeserializeObject<IEnumerable<string>>(jsonContent)!;
                return companies;
            }
            return Enumerable.Empty<string>();
        }
    }
}
