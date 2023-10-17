using HCMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HCMS.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace HCMS.Web.Controllers
{
    public class HomeController : Controller
    {

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HomeAgent()
        {
            return View();
        }

        public IActionResult HomeEmployee()
        {
            return View();
        }


        [Authorize(Roles = "Employee")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}