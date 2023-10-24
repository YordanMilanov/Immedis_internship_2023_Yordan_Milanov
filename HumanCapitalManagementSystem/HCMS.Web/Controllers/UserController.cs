using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using HCMS.Data.Models;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels;
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

        private readonly IUserService userService;
        private readonly HttpClient httpClient;

        public UserController(IUserService userService, IHttpClientFactory httpClientFactory)
        {
            this.userService = userService;
            this.httpClient = httpClientFactory.CreateClient("WebApi");
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
            string json = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make an HTTP POST request to the API endpoint
            HttpResponseMessage response = await httpClient.PostAsync("/api/users/login", content);


            //successfully passed the validation - username and password
            if (response.IsSuccessStatusCode)
            {
                //Login Logic
               
               
                //webApi
                HttpResponseMessage userResponse = await httpClient.GetAsync($"api/users/GetUserServiceModelByUsername?username={model.Username}");
                if (userResponse.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string jsonContent = await userResponse.Content.ReadAsStringAsync();

                    // Deserialize the JSON string into a UserDto object
                    UserDto? userDto = JsonConvert.DeserializeObject<UserDto>(jsonContent);

                    UserDto user = await userService.GetUserServiceModelByUsername(model.Username);

                    //Create the required claims from the userInformation
                    Claim userIdClaim = new Claim("UserId", user.Id.ToString());
                    Claim usernameClaim = new Claim("Username", user.Username.ToString());
                    //create a collection from the claims
                    var claims = new List<Claim>
                    {
                    userIdClaim,
                    usernameClaim,
                    };

                    foreach (Role role in user.Roles)
                    {
                        var roleClaim = new Claim(ClaimTypes.Role, role.Name);
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
                else
                {
                    ModelState.AddModelError("ErrorMessage", "Unexpected error occurred!");
                    return View(model);
                }
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

            try
            {
                // Serialize the model to JSON
                string json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make an HTTP POST request
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
            catch (Exception)
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
