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

namespace HCMS.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly HttpClient httpClient;
        private readonly IMapper iMapper;

        public EmployeeController(IEmployeeService employeeService, HttpClient httpClient, IMapper autoMapper)
        {
            this.employeeService = employeeService;
            this.httpClient = httpClient;
            this.iMapper = autoMapper;
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
            Guid userIdGuid = Guid.Parse(userId);

            // Make get request
            HttpResponseMessage response = await httpClient.GetAsync("/api/employee/getEmployeeUserId");
            if (response.IsSuccessStatusCode)
            {
                // Read the content as a JSON string
                string jsonContent = await response.Content.ReadAsStringAsync();
              
                // Deserialize the JSON into an EmployeeDto object
                EmployeeDto? employeeDto = JsonConvert.DeserializeObject<EmployeeDto>(jsonContent);
               
                EmployeeFormModel model = iMapper.Map<EmployeeFormModel>(employeeDto);
                //map using autoMapper TODO

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return View();
            }
            ModelState.AddModelError("ErrorMessage", "Unexpected error occurred!");
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
            try
            {
                Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")!;
                string userId = userIdClaim.Value;
                Guid userIdGuid = Guid.Parse(userId);
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
