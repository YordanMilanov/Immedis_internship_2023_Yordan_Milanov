using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HCMS.Common.RoleConstants;
using static HCMS.Common.NotificationMessagesConstants;
using HCMS.Web.ViewModels.Application;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using HCMS.Services.ServiceModels.Application;
using HCMS.Services.ServiceModels.Advert;
using HCMS.Web.ViewModels.Advert;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Employee;
using HCMS.Web.ViewModels.BaseViewModel;
using HCMS.Web.ViewModels.Employee;
using System.Reflection;

namespace HCMS.Web.Controllers
{
    public class ApplicationController : Controller
    {

        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public ApplicationController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            httpClient = httpClientFactory.CreateClient("WebApi");
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public IActionResult All()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Apply(string advertId, string employeeId, string job)
        {
            if (!HttpContext.User.HasClaim(c => c.Type == "EmployeeId"))
            {
                TempData[ErrorMessage] = "First you need to add your employee information to be able to apply for job!";
                return View("Advert", "All");
            }

            ApplicationFormModel model = new ApplicationFormModel();
            model.AdvertId = Guid.Parse(advertId);
            model.FromEmployeeId = Guid.Parse(employeeId);
            ViewData["Job"] = job;
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Apply(ApplicationFormModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Please fulfill the requirements of the input fields!";
                return View(model);
            }
 

            ApplicationDto applicationDto = mapper.Map<ApplicationDto>(model);
            string jsonContent = JsonConvert.SerializeObject(applicationDto, JsonSerializerSettingsProvider.GetCustomSettings());
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            string url = $"api/application/apply";

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            string messageResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                TempData[SuccessMessage] = messageResponse.Substring(1,messageResponse.Length - 2);
            } 
            else
            {
                TempData[ErrorMessage] = messageResponse.Substring(1, messageResponse.Length - 2);
            }
            return RedirectToAction("All", "Advert");
        }

        [HttpGet]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public async Task<IActionResult> AllByAdvert(QueryDto model,string advertId)
        {
            if (model == null)
            {
                model = new QueryDto();
            }
            model.ItemsPerPage = 10;

            string url = $"api/application/all?advertId={advertId}";
            string json = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                QueryDtoResult<ApplicationDto> responseQueryDto = JsonConvert.DeserializeObject<QueryDtoResult<ApplicationDto>>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings())!;
                ResultQueryModel<ApplicationViewModel> applicationQueryModel = mapper.Map<ResultQueryModel<ApplicationViewModel>>(responseQueryDto);
                return View("All",applicationQueryModel);
            }
            else
            {
                return View("All",model);
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public async Task<IActionResult> Accept(string id)
        {

            string url = $"api/application/accept?id={id}";

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.GetAsync(url);
            string message = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                TempData[SuccessMessage] = message;
            }
            else
            {
                TempData[WarningMessage] = message;
            }
            return RedirectToAction("All", "Advert");
        }

        [HttpGet]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public async Task<IActionResult> Decline(string id)
        {
            string url = $"api/application/decline?id={id}";

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            string message = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                TempData[SuccessMessage] = message;
            }
            else
            {
                TempData[WarningMessage] = message;
            }
            return RedirectToAction("All", "Advert");
        }
    }
}
