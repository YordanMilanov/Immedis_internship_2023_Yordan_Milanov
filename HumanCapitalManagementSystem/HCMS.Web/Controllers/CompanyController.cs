using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HCMS.Web.ViewModels.Company;
using static HCMS.Common.NotificationMessagesConstants;
using Newtonsoft.Json;
using HCMS.Services.ServiceModels.Company;
using AutoMapper;
using HCMS.Services.ServiceModels.Employee;
using System.Text;

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

        [Authorize(Roles = "AGENT,ADMIN")]
        public IActionResult Details()
        {
            return View();
        }


        [HttpGet]
        [Authorize(Roles = "EMPLOYEE")]
        public async Task<IActionResult> Select()
        {
            CompanySelectCardViewModel doubleModel = new CompanySelectCardViewModel();


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
                doubleModel.CardViewModel = model;
                doubleModel.SelectViewModel = new CompanySelectViewModel();
                //init double model


                return View(doubleModel);
            } else
            {
                doubleModel.CardViewModel = new CompanyViewModel();
                doubleModel.SelectViewModel = new CompanySelectViewModel();
                return View(doubleModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = "EMPLOYEE")]
        public async Task<IActionResult>Select(CompanySelectViewModel model)
        {
            EmployeeCompanyUpdateDto employeeDto = new EmployeeCompanyUpdateDto();
            employeeDto.CompanyName = model.Name;
            employeeDto.Id = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")!.Value);

            string json = JsonConvert.SerializeObject(employeeDto);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync("/api/employee/UpdateEmployeeCompany", content);

            if(response.IsSuccessStatusCode)
            {
                TempData[ErrorMessage] = "Your current company was succssesfully updated!";
                return View(new EmployeeCompanyUpdateDto());
            } else
            {
                ModelState.AddModelError("ErrorMessage", "Unexpecter error occured while updating your company!");
                TempData[ErrorMessage] = "Unexpected error occurred!";
                return View();
            }
        }
    }
}
