using HCMS.Web.ViewModels.WorkRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCMS.Web.Controllers
{
    public class WorkRecordController : Controller
    {
        [Authorize(Roles = "EMPLOYEE")]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        public IActionResult All()
        {
            WorkRecordAllQueryModel model = new WorkRecordAllQueryModel();
            return View(model);
        }

        [Authorize]
        public IActionResult Edit()
        {
            return View("Add");
        }
    }
}
