using AutoMapper;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels;
using HCMS.Web.ViewModels.Employee;
using HCMS.Web.ViewModels.WorkRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;

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



            return null;
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
