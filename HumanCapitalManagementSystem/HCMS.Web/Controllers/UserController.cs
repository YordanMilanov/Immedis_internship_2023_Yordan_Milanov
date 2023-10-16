using HCMS.Services;
using HCMS.Services.Interfaces;
using HCMS.Web.ViewModels.User;
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
            TempData[SuccessMessage] = "You have successfully Logged in!";
            return View(model);
            //TODO LOGIN LOGIC !
        }

        [HttpGet]
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
    }
}
