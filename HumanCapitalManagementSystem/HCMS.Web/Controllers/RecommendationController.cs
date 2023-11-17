using HCMS.Web.ViewModels.Recommendation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HCMS.Common.RoleConstants;

namespace HCMS.Web.Controllers
{
    [Authorize(Roles = ($"{AGENT},{ADMIN}"))]
    public class RecommendationController : Controller
    {
        [HttpGet]
        public IActionResult Add ()
        {
            return View();
        }

        [HttpPost]
        public Task<IActionResult> Add(RecommendationFormModel model)
        {
            return null;
        }
    }
}
