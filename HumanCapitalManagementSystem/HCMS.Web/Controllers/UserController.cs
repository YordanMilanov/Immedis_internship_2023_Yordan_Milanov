using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using AutoMapper;
using HCMS.Common.JsonConverter;
using HCMS.Services.ServiceModels.User;
using HCMS.Web.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static HCMS.Common.NotificationMessagesConstants;

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

            //validate username and password account

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new NameConverter(), new PasswordConverter(), new EmailConverter(), new RoleConverter() }
            };


            UserLoginDto userDtoToBeChecked = mapper.Map<UserLoginDto>(model);
            string json = JsonConvert.SerializeObject(userDtoToBeChecked, settings);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make a HTTP request, Consumes UserLoginDto -> Validate username and password
            HttpResponseMessage response = await httpClient.PostAsync("/api/users/loginValidate", content);

            //return if validation didnt passed
            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("ErrorMessage", errorMessage);
                return View(model);
            }
            //successfully passed the validation - username and password, and returned the UserDto
            UserDto userDto = null!;
            HttpResponseMessage getUserDtoResponse = await httpClient.GetAsync($"/api/users/UserDtoByUsername?username={model.Username}");

            if (getUserDtoResponse.IsSuccessStatusCode)
            {
                if (getUserDtoResponse.Content != null)
                {
                    // Read the response content as a string
                    string responseContent = await getUserDtoResponse.Content.ReadAsStringAsync();

                    // Deserialize the JSON content into a UserDto object
                    userDto = JsonConvert.DeserializeObject<UserDto>(responseContent)!;
                }
            


               

                //Create the required claims from the userInformation
                Claim userIdClaim = new Claim("UserId", userDto.Id.ToString()!);
                Claim usernameClaim = new Claim("Username", userDto.Username!.ToString());

                //create a collection from the claims
                var claims = new List<Claim>
                    {
                    userIdClaim,
                    usernameClaim,
                    };

                foreach (string role in userDto.Roles!)
                {
                    var roleClaim = new Claim(ClaimTypes.Role, role.ToString());
                    claims.Add(roleClaim);
                }

                //set the collection to the ClaimsIdentity
                IIdentity userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Create a ClaimsPrincipal with the claimsIdentity identity
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);

                bool rememberMe = model.RememberMe;

                AuthenticationProperties authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                };

                // Set the HttpContext.User to the userPrincipal
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal,
                    authProperties);

                //set successful message to the toastr
                string successMassage = await response.Content.ReadAsStringAsync();
                TempData[SuccessMessage] = successMassage;

                return RedirectToAction("Home", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("ErrorMessage", errorMessage);
                return View(model);
            }

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

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new NameConverter(), new PasswordConverter(), new EmailConverter() }
            };

            UserRegisterDto userDtoToBeRegistered = mapper.Map<UserRegisterDto>(model);
            string serializedRegisterDto = JsonConvert.SerializeObject(userDtoToBeRegistered, settings);
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
    }
}
