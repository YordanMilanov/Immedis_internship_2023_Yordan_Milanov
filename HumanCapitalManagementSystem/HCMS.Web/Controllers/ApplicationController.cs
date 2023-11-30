using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HCMS.Common.RoleConstants;
using static HCMS.Common.NotificationMessagesConstants;

namespace HCMS.Web.Controllers
{
    public class ApplicationController : Controller
    {

        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public ApplicationController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            httpClient = httpClientFactory.CreateClient("WebApi");
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = $"{AGENT},{ADMIN}")]
        public IActionResult All()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Apply([FromRoute]string advertId, [FromRoute]string employeeId)
        {
            string url = $"api/application/apply?advertId={advertId}?employeeId={employeeId}";

            HttpResponseMessage response = await httpClient.GetAsync(url);

            string messageResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                TempData[SuccessMessage] = messageResponse.Substring(1,messageResponse.Length - 2);
                return View("Advert", "All");
            }
            TempData[ErrorMessage] = messageResponse.Substring(1, messageResponse.Length - 2);
                return View("Advert", "All");
        }
    }
}
