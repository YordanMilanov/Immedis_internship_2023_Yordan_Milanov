using AutoMapper;
using HCMS.Services.ServiceModels.WorkRecord;
using HCMS.Web.ViewModels.WorkRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            //all companies select
            string allCompanyUrl = "api/company/AllCompanies";
            HttpResponseMessage response = await httpClient.GetAsync(allCompanyUrl);
            string jsonContent = await response.Content.ReadAsStringAsync();
            IEnumerable<string> companies = JsonConvert.DeserializeObject<IEnumerable<string>>(jsonContent)!;

            if (response.IsSuccessStatusCode)
            {
                ViewData["allCompanyNames"] = companies;
                return View();
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

            //TODO: workRecordDto needs mapping for the automapper aand the Company Name is not taken from the company Select2
            WorkRecordDto workRecordDto = mapper.Map<WorkRecordDto>(model);

            string url = "api/company/add";
            string json = JsonConvert.SerializeObject(workRecordDto);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(url, content);
           
            if(response.IsSuccessStatusCode)
            {
                string successMessage = "The work record has been successfully added!";
                ViewData["SuccessMessage"] = successMessage;
                TempData[SuccessMessage] = successMessage;
                return View();
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
    }
}
