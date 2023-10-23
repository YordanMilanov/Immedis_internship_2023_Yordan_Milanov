using HCMS.Services.Interfaces;
using HCMS.Web.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HCMS.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService userService;

        public UserApiController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(bool))]
        
        public async Task<IActionResult> RegisterUser([FromBody]UserRegisterFormModel model)
        {
           // validate email and username
            try
            {
                bool isEmailExists = await userService.IsEmailExists(model.Email);
                if (isEmailExists)
                {
                    return (BadRequest("Email is already used!"));
                }

                bool isUsernameExists = await userService.IsUsernameExists(model.Username);
                if (isUsernameExists)
                {
                    return (BadRequest("Username is already used!"));
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
            //validation passed
            await userService.RegisterUserAsync(model);
            return Ok("User has been successfully registered!");
        }
    }
}
