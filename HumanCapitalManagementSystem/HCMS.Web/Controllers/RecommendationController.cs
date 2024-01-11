using AutoMapper;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Recommendation;
using HCMS.Web.ViewModels.BaseViewModel;
using HCMS.Web.ViewModels.Recommendation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static HCMS.Common.NotificationMessagesConstants;
using static HCMS.Common.RoleConstants;

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

            string jsonContent = JsonConvert.SerializeObject(recommendationDto, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string apiUrl = "/api/recommendation/add";

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
            string responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                TempData[SuccessMessage] = responseMessage.Substring(1, responseMessage.Length - 2);

                return RedirectToAction("Home", "Home");
            }
            else
            {
                TempData[ErrorMessage] = responseMessage.Substring(1, responseMessage.Length - 2);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> All([FromRoute] string id, [FromQuery] QueryDto model)
        {
            if (model == null)
            {
                model = new QueryDto();
            }
            Guid companyId = Guid.Parse(id);

            string url = $"api/recommendation/page?companyId={companyId}";
            string json = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                QueryDtoResult<RecommendationDto> responseQueryDto = JsonConvert.DeserializeObject<QueryDtoResult<RecommendationDto>>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings())!;
                ResultQueryModel<RecommendationViewModel> recommendationQueryModel = mapper.Map<ResultQueryModel<RecommendationViewModel>>(responseQueryDto);

                string CompanyNameUrl = $"api/company/GetCompanyNameById?companyId={companyId}";
                HttpResponseMessage CompanyNameResponse = await httpClient.GetAsync(CompanyNameUrl);
                if (CompanyNameResponse.IsSuccessStatusCode)
                {
                    string companyName = await CompanyNameResponse.Content.ReadAsStringAsync();
                    TempData["CompanyName"] = companyName;
                }
                return View(recommendationQueryModel);
            }
            else
            {
                return View(model);
            }
        }
    }
}
