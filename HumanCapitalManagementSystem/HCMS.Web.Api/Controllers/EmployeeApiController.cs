using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using HCMS.Common;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Company;
using HCMS.Services.ServiceModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HCMS.Web.Api.Controllers
{


    [Route("api/employee")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly Cloudinary cloudinary;

        public EmployeeApiController(IEmployeeService employeeService, Cloudinary cloudinary)
        {
            this.employeeService = employeeService;
            this.cloudinary = cloudinary;
        }

        [HttpGet("GetEmployeeDtoByUserId")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> GetEmployeeDtoByUserId([FromQuery] string userId)
        {
            //get the employeeDtoModel
            EmployeeDto? employeeDto = await employeeService.GetEmployeeDtoByUserIdAsync(Guid.Parse(userId));

           //if employee found
            if (employeeDto != null)
            {

                //convert to json (formatting.indented -> beautifying for better human reading)
                string json = JsonConvert.SerializeObject(employeeDto, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());

                //by default content sets the http status code to 200
                return Content(json, "application/json");
            }
            //if employee not found
            return NotFound("Employee Not Found!");
        }

        [HttpPost("UpdateEmployee")]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee([FromForm]string employeeDto, [FromForm]IFormFile file)
        {
            EmployeeDto employeeDtoModel = JsonConvert.DeserializeObject<EmployeeDto>(employeeDto, JsonSerializerSettingsProvider.GetCustomSettings())!;
            employeeDtoModel.Photo = file;

            try
            {
                if (await this.employeeService.IsEmployeeEmailExistsAsync(employeeDtoModel))
                {
                    throw new Exception("Email is already used!");
                }
                if (await this.employeeService.IsEmployeePhoneNumberExistsAsync(employeeDtoModel))
                {
                    throw new Exception("Phone number is already used!");
                }
                if(employeeDtoModel.Id != Guid.Empty)
                {
                    EmployeeDto currentEmployeeDto = await employeeService.GetEmployeeDtoByIdAsync(employeeDtoModel.Id);

                    // Check if the employee already has a photo and delete it from cloudinary
                    if (!string.IsNullOrEmpty(currentEmployeeDto.PhotoUrl))
                    {
                        // Split the URL by slashes and take the last part
                        string[] urlParts = currentEmployeeDto.PhotoUrl!.Split('/');
                        string publicIdWithExtension = urlParts[urlParts.Length - 1];

                        // Remove the file extension
                        string publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);

                        var deleteParams = new DeletionParams(publicId)
                        {
                            ResourceType = ResourceType.Image
                        };

                        cloudinary.Destroy(deleteParams);
                    }
                }
                var photoFile = employeeDtoModel.Photo;

                //upload picture to cloudinary
                if (photoFile != null && photoFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        photoFile.CopyTo(memoryStream);
                        var pictureData = memoryStream.ToArray();

                        var folderName = "HCMS";
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photoFile.FileName);

                        // Upload the picture to Cloudinary
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(fileName, new MemoryStream(pictureData)),
                            Folder = folderName
                        };

                        var uploadResult = cloudinary.Upload(uploadParams);
                        employeeDtoModel.PhotoUrl = uploadResult.Url.ToString();
                    }
                }
                await employeeService.UpdateEmployeeAsync(employeeDtoModel);
                return Ok("The information has been successfully updated!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("EmployeeIdByUserId")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> GetEmployeeIdByUserId([FromQuery] string userId)
        {

            try
            {
                Guid? employeeId = await employeeService.GetEmployeeIdByUserId(Guid.Parse(userId));

                return Content(employeeId.ToString()!, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateEmployeeCompany")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> UpdateEmployeeCompany()
        {
            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
            EmployeeCompanyUpdateDto model = JsonConvert.DeserializeObject<EmployeeCompanyUpdateDto>(jsonReceived)!;
            try
            {
                await employeeService.UpdateEmployeeCompanyByCompanyName(model.Id, model.CompanyName);
                return Ok("The information has been succssesfully updated!");

            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occured while trying to update to new company!");
            }
        }

        [HttpPost("page")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize(Roles = "AGENT,ADMIN")]
        public async Task<IActionResult> GetEmployeeCurrentPage()
        {

            string jsonReceived = await new StreamReader(Request.Body).ReadToEndAsync();
            QueryDto model = JsonConvert.DeserializeObject<QueryDto>(jsonReceived)!;
            try
            {
                QueryDtoResult<EmployeeDto> employeeQueryDto = await employeeService.GetCurrentPageAsync(model);
                string jsonToSend = JsonConvert.SerializeObject(employeeQueryDto, Formatting.Indented, JsonSerializerSettingsProvider.GetCustomSettings());
                return Content(jsonToSend, "application/json");
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occured while trying to load the current page!");
            }
        }

        [HttpGet("Dismiss")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> EmployeeDismissCompany([FromQuery]string id)
        {
            try
            {
                await this.employeeService.RemoveEmployeeCompanyByIdAsync(Guid.Parse(id));
                return Ok("The company was successfully left!");
            } catch(Exception)
            {
                return BadRequest("Unexpected error occurred!");
            }
        }

        [HttpGet("fullName")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> EmployeeFullName()
        {
            var employeeId = HttpContext.Request.Query["employeeId"];

            try
            {
               string fullName = await this.employeeService.GetEmployeeFullNameById(Guid.Parse(employeeId));
                return Ok(fullName);
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error occurred!");
            }
        }
    }
}
