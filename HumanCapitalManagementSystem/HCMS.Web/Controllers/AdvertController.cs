using AutoMapper;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Company;
using HCMS.Web.ViewModels.BaseViewModel;
using HCMS.Web.ViewModels.Company;
using HCMS.Web.ViewModels.Advert;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static HCMS.Common.RoleConstants;
using HCMS.Services.ServiceModels.Advert;
using HCMS.Web.ViewModels.Education;
using static HCMS.Common.NotificationMessagesConstants;
namespace HCMS.Web.Controllers
{
    public class AdvertController : Controller
    {

        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public AdvertController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            httpClient = httpClientFactory.CreateClient("WebApi");
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> All(PageQueryModel queryModel,[FromRoute]string id)
        {
            Guid companyId = Guid.Parse(id);

            if(companyId != Guid.Empty)
            {
                if(queryModel == null)
                {
                    queryModel = new PageQueryModel();
                    queryModel.ItemsPerPage = 10;
                }

                QueryDto queryDto = mapper.Map<QueryDto>(queryModel);

                string url = $"api/advert/all?companyId={companyId}";
                string json = JsonConvert.SerializeObject(queryDto, Formatting.Indented);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                //set JWT
                string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    QueryDtoResult<CompanyDto> responseQueryDto = JsonConvert.DeserializeObject<QueryDtoResult<CompanyDto>>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings())!;
                    ResultQueryModel<CompanyViewModel> companyQueryModel = mapper.Map<ResultQueryModel<CompanyViewModel>>(responseQueryDto);
                    return View("All", companyQueryModel);
                }
                else
                {
                    return RedirectToAction("Add", "Company", new { redirect = "There is no any companies. Please first add company information!" });
                }
            }
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
        public async Task<IActionResult> Add(AdvertFormModel model)
        {
            //validate input
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            AdvertAddDto advertDto = mapper.Map<AdvertAddDto>(model);

            string jsonContent = JsonConvert.SerializeObject(advertDto, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string apiUrl = "/api/advert/add";

            //set JWT
            string tokenString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

            //if advert added successfully
            if (response.IsSuccessStatusCode)
            {
                TempData[SuccessMessage] = "Advert was successfully added!";
                return RedirectToAction("Home", "Home");
            }
            else
            {
                TempData[ErrorMessage] = response.Content.ToString();
                return View(model);
            }
        }
    }
}
