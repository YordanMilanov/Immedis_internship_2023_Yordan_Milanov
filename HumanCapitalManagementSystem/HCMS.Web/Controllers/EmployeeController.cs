using System.Security.Claims;
using AutoMapper;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels;
using HCMS.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static HCMS.Common.NotificationMessagesConstants;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using HCMS.Common.JsonConverter;
using Newtonsoft.Json.Linq;
using HCMS.Common.Structures;

namespace HCMS.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public EmployeeController(IEmployeeService employeeService, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            this.employeeService = employeeService;
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
                jsonContent.Trim();
                // Deserialize the JSON into an EmployeeDto object

                //adjust json mapping settings
                //JsonSerializerSettings settings = new JsonSerializerSettings();
                //settings.Converters.Add(new LocationConverter());
                string jsoncontent2 = "{\"Id\":\"6d69ff1f-da54-4b2d-82b6-4fe124d2dd07\",\"FirstName\":\"Homer\",\"LastName\":\"Simpson\",\"Email\":\"HomerSimpson@mail.com\",\"PhoneNumber\":\"+123456789\",\"PhotoUrl\":\"https://www.onthisday.com/images/people/homer-simpson.jpg?w=360\",\"DateOfBirth\":\"1990-01-01T00:00:00\",\"AddDate\":\"2023-10-20T22:50:35.227\",\"CompanyId\":null,\"UserId\":\"e244761d-c019-4474-b04c-14d5361e449e\",\"Location\":{\"address\":\"Springfield\",\"state\":\"Oregon\",\"country\":\"America\"}}";
                bool areTheSame = jsonContent == jsoncontent2;
                EmployeeDto employee2 = JsonConvert.DeserializeObject<EmployeeDto>(jsoncontent2, new LocationConverter())!;
                //deserializing
                EmployeeDto? employeeDto = JsonConvert.DeserializeObject<EmployeeDto>(jsonContent, new LocationConverter());
                
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
        public async Task<IActionResult> Edit(EmployeeDto model)
        {
            //validate input
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Please fulfill the requirements of the input fields!";
                return View(model);
            }

            //Validations completed
            try
            {
                Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")!;
                Guid userIdGuid = Guid.Parse(userIdClaim.Value);
                model.UserId = userIdGuid;

                await employeeService.UpdateEmployeeAsync(model);

                TempData[SuccessMessage] = "You have successfully edited your personal information!";
                return RedirectToAction("Edit");
            }
            catch (Exception)
            {
                ModelState.AddModelError("GeneralError", "An error occurred while registering the user. Please try again!");
                TempData[ErrorMessage] = "Unexpected error occurred!";

                return RedirectToAction("Edit");
            }
        }
    }
}
