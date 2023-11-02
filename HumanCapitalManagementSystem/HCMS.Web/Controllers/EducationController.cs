using AutoMapper;
using HCMS.Services.ServiceModels.Education;
using HCMS.Services.ServiceModels.Employee;
using HCMS.Web.ViewModels.Education;
using HCMS.Web.ViewModels.Employee;
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
    }
}
