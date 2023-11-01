using HCMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HCMS.Data.Models;
using Microsoft.AspNetCore.Authorization;
using HCMS.Common;
using System.Net.Http;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;

namespace HCMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient("WebApi");
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Home");
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public IActionResult Home()
        {
            if (HttpContext.User.IsInRole(RoleConstants.ADMIN))
            {
                RedirectToAction("HomeAdmin");
            } 
            else if (HttpContext.User.IsInRole(RoleConstants.AGENT))
            {
                return RedirectToAction("HomeAgent");

            }
            else if (HttpContext.User.IsInRole(RoleConstants.EMPLOYEE))
            {
                return RedirectToAction("HomeEmployee");

            }
                //something went wrong and it redirects to the index page
                return View("index");
        }

        [Authorize(Roles = RoleConstants.EMPLOYEE)]
        public IActionResult HomeEmployee()
        {
            return View();
        }

        [Authorize(Roles = RoleConstants.AGENT)]
        public IActionResult HomeAgent()
        {
            return View();
        }

        [Authorize(Roles = RoleConstants.ADMIN)]
        public IActionResult HomeAdmin()
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