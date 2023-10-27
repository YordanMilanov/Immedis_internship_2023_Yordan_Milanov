using System.Security.Claims;
using AutoMapper;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels;
using HCMS.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static HCMS.Common.NotificationMessagesConstants;
using System.Text;
using HCMS.Common;
using HCMS.Services.ServiceModels.User;
using HCMS.Data.Models;
using Microsoft.AspNetCore.Authentication;

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

        public IActionResult All()
        {
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            //get currently logged user ID
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")!;
            string userId = userIdClaim.Value;

            //set the userIdGuid as query param
            string apiUrl = $"/api/employee/GetEmployeeDtoByUserId?userId={userId}";

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
            return View();
        }

        [HttpPost]
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
            string jsonContent = JsonConvert.SerializeObject(employeeDto, JsonSerializerSettingsProvider.GetCustomSettings());
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string apiUrl = "/api/employee/UpdateEmployee";
            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

            //if employee information successfully updated
            if(response.IsSuccessStatusCode)
            {
                //if the principal has no employeeId claim -> add employee information for first time
                if (!HttpContext.User.Claims.Any(c => c.Type == "EmployeeId"))
                {
                    string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")!.Value;
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
                return RedirectToAction("Edit");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                // Read the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();
               ModelState.AddModelError("GeneralError", responseContent);
                return View(model);
            } else
            {
                ModelState.AddModelError("GeneralError", "Unexpected error occurred!");
                TempData[ErrorMessage] = "Unexpected error occurred!";
                return View(model);
            }
        }
    }
}
