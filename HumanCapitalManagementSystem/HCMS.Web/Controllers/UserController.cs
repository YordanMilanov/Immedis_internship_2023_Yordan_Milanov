using System.Security.Claims;
using System.Security.Principal;
using HCMS.Services;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.User;
using HCMS.Web.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static HCMS.Common.NotificationMessagesConstants;

namespace HCMS.Web.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
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
            Claim roleClaim = new Claim("Role", user.MaxRole.Name);

            //create a collection from the claims
            var claims = new List<Claim>
            {
                userIdClaim,
                usernameClaim,
                roleClaim,
            };

            //set the collection to the ClaimsIdentity
            IIdentity userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            // Create a ClaimsPrincipal with the claimsIdentity identity
            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);
            

            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
            };
            // Set the HttpContext.User to the userPrincipal
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);

            return RedirectToAction("HomeAgent", "Home");
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

            //validate matching passwords
            if (model.Password != model.ConfirmPassword)
            {
                TempData[ErrorMessage] = "The confirmation of the password does not match!";
                ModelState.AddModelError("PasswordDoesNotMatch", "Passwords do not match!");
                return View(model);
            }

            //validate username not existing
            if (await userService.IsUsernameExists(model.Username))
            {
                ModelState.AddModelError("UsernameExists", "This username already exists!");
                return View(model);
            }

            //validate email not existing
            if (await userService.IsEmailExists(model.Email))
            {
                ModelState.AddModelError("EmailExists", "This email already exists!");
                return View(model);
            }

            //register successful
            try
            {
                await userService.RegisterUserAsync(model);
                TempData[SuccessMessage] = "You have successfully registered!";
                TempData["LoginUsername"] = model.Username;
                return RedirectToAction("Login", "User");

            }
            catch (Exception)
            {
                ModelState.AddModelError("GeneralError", "An error occurred while registering the user. Please try again!");
                return RedirectToAction("Register", "User", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
