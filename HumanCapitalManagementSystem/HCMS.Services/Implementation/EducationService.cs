using AutoMapper;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Education;

namespace HCMS.Services.Implementation
{
    internal class EducationService : IEducationService
    {
        private readonly IMapper mapper;
        private readonly IEducationRepository educationRepository;
        private readonly ILocationRepository locationRepository;

        public EducationService(IEducationRepository educationRepository, ILocationRepository locationRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.educationRepository = educationRepository;
            this.locationRepository = locationRepository;
        }

        public async Task AddEducationAsync(EducationDto educationDto)
        {
            try
            {
                Education education = mapper.Map<Education>(educationDto);
                await educationRepository.AddEducationAsync(education);
            }
            catch (Exception)
            {
                throw new Exception("Unexpected error occurred!");
            }
        }

        public async Task<EducationDto> GetEducationDtoByIdAsync(Guid id)
        {
            try
            {
                Education education = await educationRepository.GetEducationByIdAsync(id);
                EducationDto educationDto = mapper.Map<EducationDto>(education);
                return educationDto;
            }
            catch (Exception)
            {
                throw new Exception("No education with this id was found!");
            }

        }


        public async Task EditEducationAsync(EducationDto educationDto)
        {
            try
            {
                Education educationToUpdate = await educationRepository.GetEducationByIdAsync(educationDto.Id);
                Guid locationId = educationToUpdate.Location!.Id;

                if (educationToUpdate.LocationId != null)
                {
                    educationToUpdate.University = educationDto.University;
                    educationToUpdate.FieldOfEducation = educationDto.FieldOfEducation;
                    educationToUpdate.Degree = educationDto.Degree;
                    educationToUpdate.Grade = educationDto.Grade;
                    educationToUpdate.StartDate = educationDto.StartDate;
                    educationToUpdate.EndDate = educationDto.EndDate;
                    educationToUpdate.Location = new Location()
                    {
                        Address = educationDto.Location.GetAddress(),
                        State = educationDto.Location.GetState(),
                        Country = educationDto.Location.GetCountry()
                    };
                    educationToUpdate.Location.Id = locationId;
                    educationToUpdate.LocationId = locationId;
                    await educationRepository.UpdateEducationAsync(educationToUpdate);
                }
                else
                {
                    Education updatedEducation = mapper.Map<Education>(educationDto);
                    await educationRepository.UpdateEducationAsync(updatedEducation);
                }
            }
            catch (Exception)
            {
                throw new Exception("Unexpected error occurred while trying to update your education!");
            }
        }
    }
}
