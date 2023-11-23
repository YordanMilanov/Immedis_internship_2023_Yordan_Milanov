using AutoMapper;
using HCMS.Web.ViewModels.Recommendation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static HCMS.Common.RoleConstants;
using HCMS.Services.ServiceModels.Recommendation;
using static HCMS.Common.NotificationMessagesConstants;

namespace HCMS.Web.Controllers
{
    [Authorize(Roles = ($"{AGENT},{ADMIN}"))]
    public class RecommendationController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public RecommendationController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            this.mapper = mapper;
            this.httpClient = httpClientFactory.CreateClient("WebApi");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RecommendationFormModel model)
        {
            //validate input
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Please fulfill the requirements of the input fields!";
                return View(model);
            }

            RecommendationDto recommendationDto = mapper.Map<RecommendationDto>(model);

            string jsonContent = JsonConvert.SerializeObject(recommendationDto, Formatting.Indented);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8,"application/json");

            string apiUrl = "/api/recommendation/add";

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

            if(response.IsSuccessStatusCode) 
            { 
                return RedirectToAction("Home", "Home");
            } 
            else
            {
                TempData[ErrorMessage] = response.Content.ReadAsStringAsync();
                return View(model);
            }
        }
    }
}
