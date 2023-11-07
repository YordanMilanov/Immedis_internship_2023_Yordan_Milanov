using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HCMS.Services.ServiceModels.User;
using HCMS.Web.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static HCMS.Common.NotificationMessagesConstants;
using Microsoft.IdentityModel.Tokens;
using HCMS.Services.ServiceModels.Employee;
using HCMS.Web.ViewModels.Employee;
using System.Net.Http.Headers;

namespace HCMS.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public UserController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            this.httpClient = httpClientFactory.CreateClient("WebApi");
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginFormModel model)
        {
            //validate input
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserLoginDto userDtoToBeChecked = mapper.Map<UserLoginDto>(model);
            string json = JsonConvert.SerializeObject(userDtoToBeChecked, JsonSerializerSettingsProvider.GetCustomSettings());
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");


            // Make a HTTP request, Consumes UserLoginDto -> Validate username, password and return JwtToken with claims
            HttpResponseMessage response = await httpClient.PostAsync("/api/users/loginUser", content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string tokenString = await response.Content.ReadAsStringAsync();

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                try
                {
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "http://localhost:9090",
                        ValidAudience = "http://localhost:8080",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretKeySecretKey")),
                         RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                    };

                    IEnumerable<Claim> claims = tokenHandler.ValidateToken(tokenString, tokenValidationParameters, out var validatedToken).Claims;
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    claimsIdentity.AddClaim(new Claim("JWT", tokenString));
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    bool rememberMe = model.RememberMe;

                    AuthenticationProperties authProperties = new AuthenticationProperties
                    {
                        IsPersistent = rememberMe,
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                        authProperties);

                    TempData[SuccessMessage] = "You have logged successfully!";
                    return RedirectToAction("Home", "Home");

                }
                catch (Exception ex)
                {
                    // Handle the exception when token validation fails
                    TempData[ErrorMessage] = "Token validation failed: " + ex.Message;
                    return RedirectToAction("Login", "User");
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("ErrorMessage", errorMessage);
                return View(model);
            }

            //something wrong happened!
            ModelState.AddModelError("ErrorMessage", "Unexpected error occurred!");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterFormModel model)
        {
            //validate input

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ErrorMessage", "Passwords does not match!");
                return View(model);
            }

            UserRegisterDto userDtoToBeRegistered = mapper.Map<UserRegisterDto>(model);
            string serializedRegisterDto = JsonConvert.SerializeObject(userDtoToBeRegistered, JsonSerializerSettingsProvider.GetCustomSettings());
            HttpContent content = new StringContent(serializedRegisterDto, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("/api/users/register", content);

            if (response.IsSuccessStatusCode)
            {
                // Registration was successful
                TempData[SuccessMessage] = "You have successfully registered!";
                TempData["LoginUsername"] = model.Username;
                return RedirectToAction("Login", "User");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                // Bad Request
                string errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("ErrorMessage", errorMessage);
                return View(model);
            }
            else
            {
                ModelState.AddModelError("ErrorMessage", "Unexpected error occurred!");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile() {

            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")!;
            string userId = userIdClaim.Value;

            string apiUrl = $"/api/users/GetUserViewDtoById?Id={userId}";

            string JWT = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")!.Value;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWT);

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);


            if (response.IsSuccessStatusCode)
            {


                string jsonContent = await response.Content.ReadAsStringAsync();

                UserViewDto? userViewDto = JsonConvert.DeserializeObject<UserViewDto>(jsonContent, JsonSerializerSettingsProvider.GetCustomSettings());

                if (userViewDto != null)
                {
                    UserViewModel viewModel = mapper.Map<UserViewModel>(userViewDto);
                    return View(viewModel);
                }
            }
            return RedirectToAction("Home", "Home");
        }


        [HttpGet]
        [Authorize]
        public IActionResult EditProfile(string modelJson)
        {
            UserUpdateFormModel viewModel = JsonConvert.DeserializeObject<UserUpdateFormModel>(modelJson)!;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(UserUpdateFormModel model)
        {
            //validate input

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserUpdateDto userToBeUpdated = mapper.Map<UserUpdateDto>(model);
            string jsonToSend = JsonConvert.SerializeObject(userToBeUpdated, JsonSerializerSettingsProvider.GetCustomSettings());
            HttpContent content = new StringContent(jsonToSend, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("/api/users/update", content);

            if (response.IsSuccessStatusCode)
            {
                TempData[SuccessMessage] = "You have successfully updated your profile information!";
                return RedirectToAction("Profile", "User");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                // Bad Request
                string errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("ErrorMessage", errorMessage);
                return View(model);
            }
            else
            {
                ModelState.AddModelError("ErrorMessage", "Unexpected error occurred!");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword() 
        {
            ViewData["Id"] = HttpContext.User.FindFirst("UserId")!.Value!;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(UserPasswordFormModel model) 
        {
            //validate input

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserPasswordDto userPassword = mapper.Map<UserPasswordDto>(model);
            string jsonToSend = JsonConvert.SerializeObject(userPassword, JsonSerializerSettingsProvider.GetCustomSettings());
            HttpContent content = new StringContent(jsonToSend, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("/api/users/passwordUpdate", content);

            if (response.IsSuccessStatusCode)
            {
                TempData[SuccessMessage] = "You have successfully updated your password!";
                return RedirectToAction("Profile", "User");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("ErrorMessage", errorMessage);
                return View();
            }
            else
            {
                ModelState.AddModelError("ErrorMessage", "Unexpected error occurred!");
                return View();
            }
        }
    }
}
