using HCMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HCMS.Web.ViewModels.Company;
using static HCMS.Common.NotificationMessagesConstants;
using System.Net.Http;
using HCMS.Services.ServiceModels.User;
using Newtonsoft.Json;
using HCMS.Services.ServiceModels.Company;
using AutoMapper;

namespace HCMS.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService companyService;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public CompanyController(ICompanyService companyService, HttpClient httpClient, IMapper mapper)
        {
            this.mapper = mapper;
            this.httpClient = httpClient;
            this.companyService = companyService;
        }

        [Authorize(Roles = "EMPLOYEE,ADMIN")]
        public IActionResult Edit()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Details()
        {

            //check if the user has employee information
            Claim employeeIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")!;

            //if user does not have redirect to employee information
            if(employeeIdClaim == null)
            { 
                ModelState.AddModelError("ErrorMessage", "To be able to add the current workplace, first you need to add your personal information!");
                TempData[ErrorMessage] = "Add employee information, please!";
                return View("~/Views/Employee/Edit.cshtml");
            }

            //user has employee information
            string employeeApiUrl = $"/api/company/GetCompanyDtoByEmployeeId?EmployeeId={employeeIdClaim.Value}";
            HttpResponseMessage response = await httpClient.GetAsync(employeeApiUrl);
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON content into a UserDto object
                CompanyDto companyDto = JsonConvert.DeserializeObject<CompanyDto>(responseContent)!;

                CompanyViewModel model = mapper.Map<CompanyViewModel>(companyDto);

                return View(model);
            } else
            {
                return View();
            }
        }
    }
}
