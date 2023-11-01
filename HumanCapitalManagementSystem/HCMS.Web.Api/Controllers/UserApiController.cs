using HCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HCMS.Common.JsonConverter;
using HCMS.Services.ServiceModels.User;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HCMS.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IEmployeeService employeeService;

        public UserApiController(IUserService userService, IEmployeeService employeeService)
        {
            this.userService = userService;
            this.employeeService = employeeService;
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

            UserRegisterDto model;
            try
            {
                // Deserialize the JSON
                model = JsonConvert.DeserializeObject<UserRegisterDto>(jsonReceived, JsonSerializerSettingsProvider.GetCustomSettings())!;
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


        [AllowAnonymous]
        [HttpPost("loginUser")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> loginUser()
        {
            // Read the JSON from request body
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();

            UserLoginDto model;
        
            try
            {
                model = JsonConvert.DeserializeObject<UserLoginDto>(jsonReceived, JsonSerializerSettingsProvider.GetCustomSettings())!;
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



            UserDto userDto = await userService.GetUserDtoByUsername(model.Username.ToString());

            //Create the required claims from the userInformation
            Claim userIdClaim = new Claim("UserId", userDto.Id.ToString()!);
            Claim usernameClaim = new Claim("Username", userDto.Username!.ToString());

            //create a collection of claims for the role
            var claims = new List<Claim> { userIdClaim, usernameClaim };


            Guid? employeeId = await employeeService.GetEmployeeIdByUserId(Guid.Parse(userDto.Id.ToString()!));
            if(employeeId != null)
            {
                Claim employeeIdClaim = new Claim("EmployeeId", employeeId.ToString()!);
                claims.Add(employeeIdClaim);
            }

            foreach (string role in userDto.Roles!)
            {
                Claim roleClaim = new Claim(ClaimTypes.Role, role.ToString());
                claims.Add(roleClaim);
            }

            //generate JWT

            // Define your security key and other token parameters
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretKeySecretKey")); //ITS VERY IMPORTANT HOW LONG IS THE KEY !!!
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);



            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "http://localhost:9090",
                audience: "http://localhost:8080",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: credentials
            );


            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Content(tokenString, "application/json");
        }
    }
}

