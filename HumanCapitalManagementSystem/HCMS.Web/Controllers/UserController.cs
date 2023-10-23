using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using HCMS.Data.Models;
using HCMS.Services;
using HCMS.Services.Interfaces;
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

        private readonly IUserService userService;
        private readonly HttpClient httpClient;

        public UserController(IUserService userService, HttpClient httpClient)
        {
            this.userService = userService;
            this.httpClient = httpClient;
            httpClient.BaseAddress = new Uri("https://localhost:9090");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (TempData.ContainsKey("LoginUsername"))
            {

            }
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
            //validate if username not existing
            if (!await userService.IsUsernameExists(model.Username))
            {
                ModelState.AddModelError("UsernameNotExists", "This username does not exist!");
                return View(model);
            }

            //validate not matching passwords
            if (!await userService.IsPasswordMatchByUsername(model.Username, model.Password))
            {
                TempData[ErrorMessage] = "The password does not match!";
                ModelState.AddModelError("PasswordDoesNotMatch", "The password does not match!");
                model.Password = string.Empty;
                return View(model);
            }

            //successfully passed the validation

            //Login Logic

            //set successful message to the toastr
            TempData[SuccessMessage] = "You have successfully Logged in!";

            UserServiceModel user = await userService.GetUserServiceModelByUsername(model.Username);

            //Create the required claims from the userInformation
            Claim userIdClaim = new Claim("UserId", user.Id.ToString());
            Claim usernameClaim = new Claim("Username", user.Username);
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
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);

            return RedirectToAction("Home", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [Route("/User/Register")]
        public async Task<IActionResult> Register(UserRegisterFormModel model)
        {
            //validate input
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ////validate matching passwords
            //if (model.Password != model.ConfirmPassword)
            //{
            //    TempData[ErrorMessage] = "The confirmation of the password does not match!";
            //    ModelState.AddModelError("PasswordDoesNotMatch", "Passwords do not match!");
            //    return View(model);
            //}

            ////validate username not existing
            //if (await userService.IsUsernameExists(model.Username))
            //{
            //    ModelState.AddModelError("UsernameExists", "This username already exists!");
            //    return View(model);
            //}

            ////validate email not existing
            //if (await userService.IsEmailExists(model.Email))
            //{

            //}

            ////register successful
            //try
            //{
            //    await userService.RegisterUserAsync(model);
            //    TempData[SuccessMessage] = "You have successfully registered!";
            //    TempData["LoginUsername"] = model.Username;
            //    return RedirectToAction("Login", "User");

            //}
            //catch (Exception)
            //{
            //    ModelState.AddModelError("GeneralError", "An error occurred while registering the user. Please try again!");
            //    return RedirectToAction("Register", "User", model);
            //}

            try
            {
                // Serialize the model to JSON
                string json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make an HTTP POST request to the API endpoint
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
                    // Bad Request (e.g., email or username already used)
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("ErrorMessage", "This email already exists!");
                    return View(model);
                }
                else
                {
                    // Handle other HTTP error codes as needed
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                // Handle exceptions as needed
                return View("Error");
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
