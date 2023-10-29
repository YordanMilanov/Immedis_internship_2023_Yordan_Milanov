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
using HCMS.Services.ServiceModels.Location;

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
        public async Task<IActionResult> Select(string redirect)
        {
            //first check if this is redirect from the same page
            if(redirect == "success")
            {
                TempData[SuccessMessage] = "Your current company was succssesfully updated!";
            }
            else if(redirect == "error")
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
            if(employeeIdClaim == null)
            { 
                ModelState.AddModelError("ErrorMessage", "To be able to add the current workplace, first you need to add your personal information!");
                TempData[ErrorMessage] = "Add employee information, please!";
                return View("~/Views/Employee/Edit.cshtml");
            }

            //user has employee information
            string employeeCompanyApiUrl = $"/api/company/GetCompanyDtoByEmployeeId?EmployeeId={employeeIdClaim.Value}";
            HttpResponseMessage response = await httpClient.GetAsync(employeeCompanyApiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                CompanyDto companyDto = JsonConvert.DeserializeObject<CompanyDto>(responseContent)!;
                CompanyViewModel model = mapper.Map<CompanyViewModel>(companyDto);

                //check if company has location and take it
                string LocationDtoApiUrl = $"/api/location/GetLocationById?id={companyDto.LocationId}";
                HttpResponseMessage locationResponse = await httpClient.GetAsync(LocationDtoApiUrl);

                if(locationResponse.IsSuccessStatusCode)
                {
                    string locationResponseContent = await locationResponse.Content.ReadAsStringAsync();
                    LocationDto locationDto = JsonConvert.DeserializeObject<LocationDto>(locationResponseContent)!;
                    model.Country = locationDto.Country!;
                    model.State = locationDto.State!;
                    model.Address = locationDto.Address!;
                }

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
        [Authorize(Roles = "EMPLOYEE")]
        public async Task<IActionResult>Select(CompanySelectViewModel model)
        {
            EmployeeCompanyUpdateDto employeeDto = new EmployeeCompanyUpdateDto();
            employeeDto.CompanyName = model.Name;
            employeeDto.Id = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")!.Value);

            string json = JsonConvert.SerializeObject(employeeDto);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync("/api/employee/UpdateEmployeeCompany", content);

            if (response.IsSuccessStatusCode)
            {

                Claim employeeIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")!;
                string employeeApiUrl = $"/api/company/GetCompanyDtoByEmployeeId?EmployeeId={employeeIdClaim.Value}";
                HttpResponseMessage CompanyDtoResponse = await httpClient.GetAsync(employeeApiUrl);

                CompanyDto companyDto = JsonConvert.DeserializeObject<CompanyDto>(await CompanyDtoResponse.Content.ReadAsStringAsync())!;
                CompanySelectCardViewModel returnModel = new CompanySelectCardViewModel();

                returnModel.CardViewModel = mapper.Map<CompanyViewModel>(companyDto);
                returnModel.SelectViewModel = new CompanySelectViewModel();

                //the 3rd parameter is the route attribute for redirect and in the get method if it is success it adds tempdata(check in get method)
                return RedirectToAction("Select", "Company", new { redirect = "success"});
            } else
            {
                return RedirectToAction("Select", "Company", new { redirect = "error"});
            }
        }
    }
}
