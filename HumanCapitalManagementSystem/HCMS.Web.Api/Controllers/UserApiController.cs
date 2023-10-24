using HCMS.Services.Interfaces;
using HCMS.Web.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HCMS.Services.ServiceModels;
using Newtonsoft.Json;

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
        [ProducesResponseType(200)]
        public async Task<IActionResult> RegisterUser([FromBody]UserRegisterFormModel model)
        {
           // validate email and username
            try
            {
                
                if (await userService.IsEmailExists(model.Email))
                {
                    return (BadRequest("Email is already used!"));
                }


                if (await userService.IsUsernameExists(model.Username))
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

        [HttpPost("login")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> ValidateLoginUser([FromBody] UserLoginFormModel model)
        {
            //validate the name
            if (!(await userService.IsUsernameExists(model.Username)))
            {
                return BadRequest("User name does not exists!");
            }

            //validate password
            if (!(await userService.IsPasswordMatchByUsername(model.Username, model.Password)))
            {
                return BadRequest("Wrong password!");
            }

            //successful validation
            return Ok("You have Successfully logged!");
        }

        [HttpPost("GetUserServiceModelByUsername")]
        [Consumes("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public async Task<IActionResult> GetUserServiceModelByUsername([FromQuery] string username)
        {
            UserDto userDto = await userService.GetUserServiceModelByUsername(username);

            if (userDto != null)
            {
                return Ok(userDto);
            }

            return NotFound("Employee Not Found!");
        }
    }
}
