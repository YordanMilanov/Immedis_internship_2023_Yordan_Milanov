using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HCMS.Common.RoleConstants;

namespace HCMS.Web.Controllers
{
    public class ApplicationController : Controller
    {
        [HttpGet]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public IActionResult All()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Apply()
        {
            return View();
        }
    }
}
