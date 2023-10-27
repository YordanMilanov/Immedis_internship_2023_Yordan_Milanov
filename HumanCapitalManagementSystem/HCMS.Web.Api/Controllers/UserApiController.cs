using HCMS.Services.Interfaces;
using HCMS.Web.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HCMS.Common.JsonConverter;
using HCMS.Services.ServiceModels.User;

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
        public async Task<IActionResult> RegisterUser()
        {
            // Read the JSON from request body
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();

            // Deserialize the JSON
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new NameConverter(), new PasswordConverter(), new EmailConverter(), new RoleConverter() }
            };

            UserRegisterDto model;
            try
            {
                model = JsonConvert.DeserializeObject<UserRegisterDto>(jsonReceived, settings)!;
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON data");
            }

            // validate email and username
            try
            {

                if (await userService.IsEmailExists(model.Email.ToString()))
                {
                    return (BadRequest("Email is already used!"));
                }


                if (await userService.IsUsernameExists(model.Username.ToString()))
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

        [HttpPost("loginValidate")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> ValidateLoginUser()
        {
            // Read the JSON from request body
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();

            // Deserialize the JSON
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new NameConverter(), new PasswordConverter(), new EmailConverter(), new RoleConverter() }
            };

            UserLoginDto model;
            try
            {
                model = JsonConvert.DeserializeObject<UserLoginDto>(jsonReceived, settings)!;
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON data");
            }

            //validate the name
            if (!(await userService.IsUsernameExists(model.Username.ToString())))
            {
                return BadRequest("User name does not exists!");
            }

            //validate password
            if (!(await userService.IsPasswordMatchByUsername(model.Username.ToString(), model.Password.ToString()!)))
            {
                return BadRequest("Wrong password!");
            }

            //username and password validated

            return Ok("Succsessfully logged!");
        }

        [HttpGet("UserDtoByUsername")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetUserDtoByUsername([FromQuery] string username)
        {

            UserDto userDto = await userService.GetUserDtoByUsername(username);

            if(userDto != null)
            {
                return Ok(userDto);
            } else
            {
                return BadRequest("Unexpected error occurred!");
            }
        }
   
    }
}

