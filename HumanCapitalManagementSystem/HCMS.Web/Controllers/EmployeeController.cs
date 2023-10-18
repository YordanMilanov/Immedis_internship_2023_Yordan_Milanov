using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCMS.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
