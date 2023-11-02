using HCMS.Services.ServiceModels.Education;

namespace HCMS.Services.Interfaces
{
    public interface IEducationService
    {
        Task AddEducationAsync(EducationDto educationDto);

        Task EditEducationAsync(EducationDto educationDto);

        Task<EducationDto> GetEducationDtoByIdAsync(Guid id);

        Task<List<EducationDto>> GetAllEducationDtosByEmployeeIdAsync(Guid employeeId);

    }
}
