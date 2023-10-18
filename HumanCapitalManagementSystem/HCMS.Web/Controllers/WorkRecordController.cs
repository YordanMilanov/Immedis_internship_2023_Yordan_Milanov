using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCMS.Web.Controllers
{
    public class WorkRecordController : Controller
    {
        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        public IActionResult All()
        {
            return View();
        }
    }
}
