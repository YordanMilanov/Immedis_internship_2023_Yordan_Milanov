using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HCMS.Common.RoleConstants;

namespace HCMS.Web.Controllers
{
    public class AdvertController : Controller
    {

        [HttpGet]
        public IActionResult All()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public IActionResult Add(string post)
        {
            return View();
        }
    }
}
