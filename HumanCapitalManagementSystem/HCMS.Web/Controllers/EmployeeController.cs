using System.Security.Claims;
using AutoMapper;
using HCMS.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static HCMS.Common.NotificationMessagesConstants;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using HCMS.Services.ServiceModels.Employee;
using System.Net.Http.Headers;
using HCMS.Services.ServiceModels.Company;
using HCMS.Web.ViewModels.Company;
using HCMS.Common;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Web.ViewModels.BaseViewModel;

namespace HCMS.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public EmployeeController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            httpClient = httpClientFactory.CreateClient("WebApi");
            this.mapper = mapper;
        }

        [Authorize(Roles = $"{RoleConstants.AGENT},{RoleConstants.ADMIN}")]
        public async Task<IActionResult> All(QueryDto model)
        {
            if (model == null)
            {
                model = new QueryDto();
            }

            string url = "api/employee/page";
            string json = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                QueryDtoResult<EmployeeDto> responseQueryDto = JsonConvert.DeserializeObject<QueryDtoResult<EmployeeDto>>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings())!;
                ResultQueryModel<EmployeeViewModel> employeeQueryModel = mapper.Map<ResultQueryModel<EmployeeViewModel>>(responseQueryDto);
                return View(employeeQueryModel);
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string? redirect)
        {
            //check if it is redirect
            if(redirect != null)
            {
                TempData[WarningMessage] = redirect;
                return View(new EmployeeFormModel());
            }

            //get currently logged user ID
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")!;
            string userId = userIdClaim.Value;

            //set the userIdGuid as query param
            string apiUrl = $"/api/employee/GetEmployeeDtoByUserId?userId={userId}";
           
            //set the JWT to the httpClient
            string JWT = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWT);

            // Make get request
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);


            if (response.IsSuccessStatusCode)
            {


                // Read the content as a JSON string
                string jsonContent = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON into an EmployeeDto object

                //deserializing (if not working check the request from the other side) the JsonSerializerSettingsProvider is custom made in common
                EmployeeDto? employeeDto = JsonConvert.DeserializeObject<EmployeeDto>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings());
                
                //check if employee is not empty and attach it as a view model
                if (employeeDto != null)
                {
                  EmployeeFormModel model = mapper.Map<EmployeeFormModel>(employeeDto);
                  return View(model);
                }
            }
            return View(new EmployeeFormModel());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EmployeeFormModel model)
        {
            //validate input
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Please fulfill the requirements of the input fields!";
                return View(model);
            }
            //Validations completed
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")!;

            EmployeeDto employeeDto = mapper.Map<EmployeeDto>(model);
            employeeDto.UserId = Guid.Parse(userIdClaim.Value);

            using (var form = new MultipartFormDataContent())
            {
                // Add the file content
                if (model.Photo != null)
                {
                    // Use a MemoryStream without disposing it here
                    var stream = new MemoryStream();
                    await model.Photo.CopyToAsync(stream);
                    stream.Position = 0;
                    form.Add(new StreamContent(stream), "file", model.Photo.FileName);
                }

                // Convert EmployeeDto to JSON and add it as a StringContent
                employeeDto.Photo = null;
                string jsonContent = JsonConvert.SerializeObject(employeeDto, JsonSerializerSettingsProvider.GetCustomSettings());
                form.Add(new StringContent(jsonContent, Encoding.UTF8, "application/json"), "employeeDto");

                string apiUrl = "/api/employee/UpdateEmployee";

                // Set JWT
                string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, form);

                //if employee information successfully updated
                if (response.IsSuccessStatusCode)
                {
                    //if the principal has no employeeId claim -> add employee information for first time
                    if (!HttpContext.User.Claims.Any(c => c.Type == "EmployeeId"))
                    {
                        string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")!.Value;

                        //JWT is already set
                        //set the employeeId to the claims of the user
                        string employeeApiUrl = $"/api/employee/EmployeeIdByUserId?UserId={userId}";
                        HttpResponseMessage employeeIdResponse = await httpClient.GetAsync(employeeApiUrl);
                        if (employeeIdResponse.IsSuccessStatusCode)
                        {
                            var userClaims = new List<Claim>(HttpContext.User.Claims);

                            // Add / update claims
                            string employeeId = await employeeIdResponse.Content.ReadAsStringAsync();
                            Claim employeeIdClaim = new Claim("EmployeeId", employeeId);
                            userClaims.RemoveAll(c => c.Type == "EmployeeId");
                            userClaims.Add(employeeIdClaim);

                            ClaimsIdentity updatedIdentity = new ClaimsIdentity(userClaims, HttpContext.User.Identity!.AuthenticationType, ClaimTypes.Name, ClaimTypes.Role);

                            ClaimsPrincipal updatedPrincipal = new ClaimsPrincipal(updatedIdentity);

                            await HttpContext.SignInAsync(updatedPrincipal);
                        }
                    }

                    TempData[SuccessMessage] = "You have successfully edited your personal information!";
                    return RedirectToAction("Home", "Home");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("GeneralError", responseContent);
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("GeneralError", "Unexpected error occurred!");
                    TempData[ErrorMessage] = "Unexpected error occurred!";
                    return View(model);
                }
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Dismiss(string id)
        {
            //set the userIdGuid as query param
            string apiUrl = $"/api/employee/Dismiss?id={id}";

            //set the JWT to the httpClient
            string JWT = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWT);

            // Make get request
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);


            if (response.IsSuccessStatusCode)
            {
                TempData[SuccessMessage] = "You have successfully left the company!";
                return RedirectToAction("Home", "Home");
            } 
            else
            {
                TempData[ErrorMessage] = await response.Content.ReadAsStringAsync();
                return RedirectToAction("Company", "Select");
            }
        }


    }
}
