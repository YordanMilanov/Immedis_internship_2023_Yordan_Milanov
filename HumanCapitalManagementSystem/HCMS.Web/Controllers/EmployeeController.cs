using System.Security.Claims;
using HCMS.Common;
using HCMS.Services;
using HCMS.Services.Interfaces;
using HCMS.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HCMS.Common.NotificationMessagesConstants;

namespace HCMS.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public IActionResult All()
        {
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")!;
            string userId = userIdClaim.Value;
            Guid userIdGuid = Guid.Parse(userId);

            EmployeeFormModel? model = await employeeService.GetEmployeeFormModelByUserIdAsync(userIdGuid);

            if (model == null)
            {
                return View();
            }
            else
            {
                return View(model);
            }
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
