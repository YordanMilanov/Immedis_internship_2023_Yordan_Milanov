using Microsoft.AspNetCore.Mvc;

namespace HCMS.Web.Controllers
{
    public class ApplicationController : Controller
    {
        public IActionResult All()
        {
            return View();
        }

        public IActionResult Apply()
        {
            return View();
        }
    }
}
