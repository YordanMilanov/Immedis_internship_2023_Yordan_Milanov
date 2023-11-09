using AutoMapper;
using HCMS.Common;
using HCMS.Services.ServiceModels.Education;
using HCMS.Web.ViewModels.Education;
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
        public async Task<IActionResult> Edit([FromQuery]string? educationId)
        {


            if(educationId != null) {
                string url = $"/api/education/EducationsDtoById?educationId={educationId}";

                //set JWT
                string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    EducationDto educationDto = JsonConvert.DeserializeObject<EducationDto>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings())!;
                    EducationFormModel educationFormModel = mapper.Map<EducationFormModel>(educationDto);
                    return View(educationFormModel);
                } 
                else
                {
                    TempData[ErrorMessage] = "No education was found!";
                    return View();
                }
            }
            else
            {
                //check if the current user has employeeId
                bool hasEmployeeId = HttpContext.User.Claims.Any(c => c.Type == "EmployeeId");
                if (!hasEmployeeId)
                {
                    return RedirectToAction("Edit", "Employee", new { redirect = "You must first add personal information to be able to add education information!" });
                }

                //Adding
                return View(new EducationFormModel());
            }
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

            //if employee education information successfully updated
            if (response.IsSuccessStatusCode)
            {
                TempData[SuccessMessage] = "Education information has been successfully added!";
                return RedirectToAction("All", "Education", new { id = employeeId});
            }
            else
            {
                TempData[ErrorMessage] = response.Content.ToString();
                return View(new EducationFormModel());
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> All(EducationPageModel model,string? id,string? redirectMessage)
        {
            if(redirectMessage != null)
            {
                TempData[InformationMessage] = redirectMessage;
            }

            if (model == null)
            {
                model = new EducationPageModel();
            }

            if(!HttpContext.User.IsInRole(RoleConstants.AGENT) && !HttpContext.User.IsInRole(RoleConstants.ADMIN))
            {
                if (!HttpContext.User.Claims.Any(c => c.Type == "EmployeeId"))
                {
                    return RedirectToAction("Edit", "Employee", new { redirect = "You have no personal information. Please first add personal information!" });
                }
            }

            string url = $"api/education/EducationsPageByEmployeeId?employeeId={id}&page={model.Page}";

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
                string countUrl = $"api/education/EducationsCountByEmployeeId?employeeId={id}";
                HttpResponseMessage countResponse = await httpClient.GetAsync(countUrl);
                if (countResponse.IsSuccessStatusCode)
                {
                    string jsonCountContent = await countResponse.Content.ReadAsStringAsync();
                    string educationsCount = JsonConvert.DeserializeObject<string>(jsonCountContent)!;
                    model.TotalEducations = int.Parse(educationsCount);
                }

                //get employee name(first + last)
                string employeeNameUrl = $"api/employee/fullName?employeeId={id}";
                HttpResponseMessage employeeNameResponse = await httpClient.GetAsync(employeeNameUrl);
                if (employeeNameResponse.IsSuccessStatusCode)
                {
                    string nameResponse = await employeeNameResponse.Content.ReadAsStringAsync();
                    ViewData["employeeName"] = nameResponse.Substring(1, nameResponse.Length - 2);
                }
               
                ViewData["employeeId"] = id;
                ViewData["username"] = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Username")!.Value;

                return View(model);
            } 
            else
            {
                TempData[WarningMessage] = "No education information was found! Please first add your educations.";
                return View("Edit");
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            string url = $"api/education/DeleteById?EducationId={id}";

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("All", new { redirectMessage = "The education record was successfully deleted!" });
            } 
            else
            {
                return RedirectToAction("All", new { redirectMessage = "Unexpected error occurred while trying to deleted!" });
            }
        }
    }
}
