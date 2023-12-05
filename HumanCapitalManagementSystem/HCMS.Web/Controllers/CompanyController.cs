using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HCMS.Web.ViewModels.Company;
using static HCMS.Common.NotificationMessagesConstants;
using static HCMS.Common.RoleConstants;
using Newtonsoft.Json;
using HCMS.Services.ServiceModels.Company;
using AutoMapper;
using HCMS.Services.ServiceModels.Employee;
using System.Text;
using System.Net.Http.Headers;
using HCMS.Web.ViewModels.BaseViewModel;
using HCMS.Services.ServiceModels.BaseClasses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HCMS.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public CompanyController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            this.mapper = mapper;
            this.httpClient = httpClientFactory.CreateClient("WebApi");
        }

        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public IActionResult Details()
        {
            return View();
        }


        [HttpGet]
        [Authorize(Roles = $"{ADMIN}")]
        public async Task<IActionResult> Select(string redirect)
        {
            //first check if this is redirect from the same page
            if (redirect == "success")
            {
                TempData[SuccessMessage] = "Your current company was succssesfully updated!";
            }
            else if (redirect == "error")
            {
                TempData[ErrorMessage] = "Unexpected error occurred!";
            }

            //if the user has no employee information redirect to add employee information
            if (!HttpContext.User.Claims.Any(c => c.Type == "EmployeeId"))
            {
                return RedirectToAction("Edit", "Employee", new { redirect = "Please first add your employee information to be able to add company information!" });
            }

            CompanySelectCardViewModel doubleModel = new CompanySelectCardViewModel();


            //check if the user has employee information
            Claim employeeIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")!;

            //if user does not have redirect to employee information
            if (employeeIdClaim == null)
            {
                ModelState.AddModelError("ErrorMessage", "To be able to add the current workplace, first you need to add your personal information!");
                TempData[ErrorMessage] = "Add employee information, please!";
                return RedirectToAction("Edit", "Employee");
            }

            //user has employee information
            string employeeCompanyApiUrl = $"/api/company/GetCompanyDtoByEmployeeId?EmployeeId={employeeIdClaim.Value}";

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.GetAsync(employeeCompanyApiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                CompanyDto companyDto = JsonConvert.DeserializeObject<CompanyDto>(responseContent, JsonSerializerSettingsProvider.GetCustomSettings())!;
                CompanyViewModel model = mapper.Map<CompanyViewModel>(companyDto);


                //init double model
                doubleModel.CardViewModel = model;
                doubleModel.SelectViewModel = new CompanySelectViewModel();

                return View(doubleModel);
            } else
            {
                doubleModel.CardViewModel = new CompanyViewModel();
                doubleModel.SelectViewModel = new CompanySelectViewModel();
                return View(doubleModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{ADMIN}")]
        public async Task<IActionResult> Select(CompanySelectViewModel model)
        {
            EmployeeCompanyUpdateDto employeeDto = new EmployeeCompanyUpdateDto();
            employeeDto.CompanyName = model.Name;
            employeeDto.Id = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")!.Value);

            string json = JsonConvert.SerializeObject(employeeDto);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.PostAsync("/api/employee/UpdateEmployeeCompany", content);

            if (response.IsSuccessStatusCode)
            {

                var existingClaims = HttpContext.User.Claims.ToList();
                existingClaims.RemoveAll(c => c.Type == "EmployeeCompany");
                var updatedClaim = new Claim("EmployeeCompany", model.Name);
                existingClaims.Add(updatedClaim);

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(existingClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);


                return RedirectToAction("Select", "Company", new { redirect = "success" });
            } else
            {
                return RedirectToAction("Select", "Company", new { redirect = "error" });
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public async Task<IActionResult> All(PageQueryModel model)
        {
            if (model == null)
            {
                model = new PageQueryModel();
            }


            QueryDto queryDto = mapper.Map<QueryDto>(model);

            string url = "api/company/all";
            string json = JsonConvert.SerializeObject(queryDto, Formatting.Indented);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                QueryDtoResult<CompanyDto> responseQueryDto = JsonConvert.DeserializeObject<QueryDtoResult<CompanyDto>>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings())!;
                ResultQueryModel<CompanyViewModel> companyQueryModel = mapper.Map<ResultQueryModel<CompanyViewModel>>(responseQueryDto);
                return View("All", companyQueryModel);
            }
            else
            {
                return RedirectToAction("Add", "Company", new { redirect = "There is no any companies. Please first add company information!" });
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public async Task<IActionResult> Edit(string? id, string? redirectMessage)
        {
            if(id == null)
            {
                return View(new CompanyFormModel());
            } 
            else
            {
                string url = $"api/company/GetCompanyById?id={id}";

                //set JWT
                string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    CompanyDto companyDto = JsonConvert.DeserializeObject<CompanyDto>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings())!;
                    CompanyFormModel model = mapper.Map<CompanyFormModel>(companyDto);

                    return View(model);

                } 
                else
                {
                    TempData[WarningMessage] = "No company information was found";
                    return View("All","Company");
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public async Task<IActionResult> Edit(CompanyFormModel model)
        {
            //validate input
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            CompanyDto companyDto = mapper.Map<CompanyDto>(model);
            string json = JsonConvert.SerializeObject(companyDto, JsonSerializerSettingsProvider.GetCustomSettings());
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
            HttpResponseMessage response = await httpClient.PostAsync("/api/company/updateCompany", content);

            if (response.IsSuccessStatusCode)
            {
                TempData[SuccessMessage] = "Company information was successfully updated!";
                return RedirectToAction("All", "Company");
            }
            else
            {
                string responseMessage = await response.Content.ReadAsStringAsync();
                TempData[ErrorMessage] = responseMessage.Substring(1, responseMessage.Length - 2);
                ModelState.AddModelError("ErrorMessage", responseMessage.Substring(1, responseMessage.Length - 2));
                return View("Edit", model);
            }
        }
    }
}

