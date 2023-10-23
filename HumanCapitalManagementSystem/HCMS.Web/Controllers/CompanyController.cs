using HCMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HCMS.Web.ViewModels.Company;

namespace HCMS.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [Authorize(Roles = "EMPLOYEE,ADMIN")]
        public IActionResult Edit()
        {
            return View();
        }

        public async Task<IActionResult> Details()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")!;
            string userId = userIdClaim.Value;
            Guid userIdGuid = Guid.Parse(userId);

            CompanyViewModel model = await companyService.GetCompanyByUserId(userIdGuid);

            return View(model);
        }
    }
}
