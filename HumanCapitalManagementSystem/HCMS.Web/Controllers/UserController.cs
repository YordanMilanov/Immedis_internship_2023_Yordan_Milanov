using HCMS.Services;
using HCMS.Services.Interfaces;
using HCMS.Web.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string TODO)
        {
            return View();
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
