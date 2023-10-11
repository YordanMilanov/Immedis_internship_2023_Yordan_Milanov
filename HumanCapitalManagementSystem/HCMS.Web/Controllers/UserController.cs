using Microsoft.AspNetCore.Mvc;

namespace HCMS.Web.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string TODO)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string TODO)
        {
            return View();
        }
    }
}
