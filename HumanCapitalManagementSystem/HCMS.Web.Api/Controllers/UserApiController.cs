using HCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HCMS.Services.ServiceModels.User;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HCMS.Services.ServiceModels.Employee;
using HCMS.Services.ServiceModels.WorkRecord;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Common;

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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("loginValidate")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> ValidateLoginUser()
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

            //username and password validated

            return Ok("Succsessfully logged!");
        }

        [HttpGet("UserDtoByUsername")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> GetUserDtoByUsername([FromQuery] string username)
        {

            UserDto userDto = await userService.GetUserDtoByUsernameAsync(username);

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



            UserDto userDto = await userService.GetUserDtoByUsernameAsync(model.Username.ToString());

            //Create the required claims from the userInformation
            Claim userIdClaim = new Claim("UserId", userDto.Id.ToString()!);
            Claim usernameClaim = new Claim("Username", userDto.Username!.ToString());

            //create a collection of claims for the role
            var claims = new List<Claim> { userIdClaim, usernameClaim };



            EmployeeDto? employeeDto = await employeeService.GetEmployeeDtoByUserIdAsync(Guid.Parse(userDto.Id.ToString()!));
            if (employeeDto != null)
            {
                Claim employeeIdClaim = new Claim("EmployeeId", employeeDto.Id.ToString()!);
                claims.Add(employeeIdClaim);

                Claim employeeNameClaim = new Claim("EmployeeName", $"{employeeDto.FirstName} {employeeDto.LastName}");
                claims.Add(employeeNameClaim);

                if (employeeDto.CompanyName != null)
                {
                    Claim employeeCompanyName = new Claim("EmployeeCompany", employeeDto.CompanyName!);
                    claims.Add(employeeCompanyName);
                }

                if (employeeDto.CompanyId != null)
                {
                    Claim employeeCompanyId = new Claim("EmployeeCompanyId", employeeDto.CompanyId!.ToString()!);
                    claims.Add(employeeCompanyId);
                }
            }

            foreach (string role in userDto.Roles!)
            {
                Claim roleClaim = new Claim(ClaimTypes.Role, role.ToString());
                claims.Add(roleClaim);
            }

            //generate JWT

            JwtSecurityToken token = GenerateToken(claims);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Content(tokenString, "application/json");
        }

        [HttpGet("GetUserViewDtoById")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> GetUserViewDtoById([FromQuery] string Id)
        {
            try
            {
                UserViewDto userViewDto = await this.userService.GetUserViewDtoByIdAsync(Guid.Parse(Id));
                return Ok(userViewDto);
            } 
            catch(Exception)
            {
               return BadRequest("No user was found!");
            }
        }


        [HttpPost("update")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> UpdateUser()
        {
            // Read the JSON from request body
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();

            UserUpdateDto model;
            try
            {
                // Deserialize the JSON
                model = JsonConvert.DeserializeObject<UserUpdateDto>(jsonReceived)!;
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON data");
            }

            UserViewDto currentUser = await this.userService.GetUserViewDtoByIdAsync(model.Id);


            // validate email and username
            try
            {
                if (currentUser.Email != model.Email)
                {
                    if (await userService.IsEmailExists(model.Email.ToString()))
                    {
                        return (BadRequest("Email is already used!"));
                    }
                }

                if (currentUser.Username != model.Username)
                {
                    if (await userService.IsUsernameExists(model.Username.ToString()))
                    {
                        return BadRequest("Username is already used!");
                    }
                }

                //fields are valid
                //update
                await userService.UpdateUserAsync(model);
                return Ok("Your profile information was successfully updated!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("passwordUpdate")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> UpdatePassword()
        {
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();

            UserPasswordDto model;
            try
            {
                // Deserialize the JSON
                model = JsonConvert.DeserializeObject<UserPasswordDto>(jsonReceived)!;
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON data");
            }

            try
            {
                await userService.UpdatePasswordAsync(model);
                return Ok("Your password was successfully updated!");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while updating your password!");
            }
        }

        [HttpPost("allPage")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize(Roles = $"{RoleConstants.AGENT}, {RoleConstants.ADMIN}")]
        public async Task<IActionResult> GetAllCurrentPage([FromBody]QueryDto model)
        {
            try
            {
                QueryDtoResult<UserViewDto> userQueryDto = await userService.GetUsersCurrentPageAsync(model);
                string jsonString = JsonConvert.SerializeObject(userQueryDto,Formatting.Indented,JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(jsonString, "application/json");

            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred while trying to load the page!");
            }
        }

        [HttpPatch("updateRole")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize(Roles = RoleConstants.ADMIN)]
        public async Task<IActionResult> UpdateRole([FromBody] UserRoleUpdateDto model)
        {
            try
            {
                await this.userService.UpdateUserRoleAsync(model);
                return Ok("The user roles were successfully updated!");
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        private static JwtSecurityToken GenerateToken(List<Claim> claims)
        {
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
            return token;
        }
    }
}

