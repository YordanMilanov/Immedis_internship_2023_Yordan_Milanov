using AutoMapper;
using HCMS.Services.ServiceModels.Education;
using HCMS.Services.ServiceModels.Employee;
using HCMS.Services.ServiceModels.WorkRecord;
using HCMS.Web.ViewModels.Education;
using HCMS.Web.ViewModels.Employee;
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
    public class EducationController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public EducationController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            this.mapper = mapper;
            this.httpClient = httpClientFactory.CreateClient("WebApi");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit()
        {
            //check if the current user has employeeId
            bool hasEmployeeId = HttpContext.User.Claims.Any(c => c.Type == "EmployeeId");
            if (!hasEmployeeId)
            {
                return RedirectToAction("Edit", "Employee", new { redirect = "You must first add personal information to be able to add education information!" });
            }
            return View();
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EducationFormModel model)
        {
            //validate input
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            //get currently logged user ID
            Claim employeeIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")!;
            Guid employeeId = Guid.Parse(employeeIdClaim.Value);
            EducationDto educationDto = mapper.Map<EducationDto>(model);
            educationDto.EmployeeId = employeeId;
            string jsonContent = JsonConvert.SerializeObject(educationDto, JsonSerializerSettingsProvider.GetCustomSettings());
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string apiUrl = "/api/education/EditEducation";

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

            //if employee information successfully updated
            if (response.IsSuccessStatusCode)
            {
                TempData[SuccessMessage] = "Education information has been succssesfully added!";
                return View();
            } else
            {
                TempData[ErrorMessage] = response.Content.ToString();
                return View();

            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> All(EducationPageModel model)
        {
            if (model == null)
            {
                model = new EducationPageModel();
            }

            if (!HttpContext.User.Claims.Any(c => c.Type == "EmployeeId"))
            {
                return RedirectToAction("Edit", "Employee", new { redirect = "You have no personal information. Please first add personal information!" });
            }

            string employeeId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")!.Value;
            
            string url = $"api/education/EducationsPageByEmployeeId?employeeId={employeeId}&page={model.Page}";

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                List<EducationDto> educationDtos = JsonConvert.DeserializeObject<List<EducationDto>>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings())!;
                List<EducationViewModel> educationViewModels = educationDtos.Select(e => mapper.Map<EducationViewModel>(e)).ToList();
                model.Educations = educationViewModels;

                //take count of all educations
                string countUrl = $"api/education/EducationsCountByEmployeeId?employeeId={employeeId}";
                HttpResponseMessage countResponse = await httpClient.GetAsync(url);
                if (countResponse.IsSuccessStatusCode)
                {
                    string jsonCountContent = await response.Content.ReadAsStringAsync();
                    int educationsCount = JsonConvert.DeserializeObject<int>(jsonCountContent, JsonSerializerSettingsProvider.GetCustomSettings())!;
                    model.TotalPage = educationsCount;
                }
                    ViewData["username"] = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Username")!.Value;

                return View(model);
            } 
            else
            {
                TempData[WarningMessage] = "No education information was found! Please first add your educations.";
                return View("Edit");
            }
        }
    }
}
