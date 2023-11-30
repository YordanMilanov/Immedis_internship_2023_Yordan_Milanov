using Microsoft.AspNetCore.Mvc;

namespace HCMS.Web.Api.Controllers
{
    public class ApplicationApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
