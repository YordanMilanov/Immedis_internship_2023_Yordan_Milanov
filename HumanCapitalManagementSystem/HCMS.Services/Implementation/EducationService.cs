using AutoMapper;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Education;
using Microsoft.EntityFrameworkCore;
using System;

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
                Education educationInfo = mapper.Map<Education>(educationDto);
                await educationRepository.UpdateEducationAsync(educationInfo);
            }
            catch (Exception)
            {
                throw new Exception("Unexpected error occurred while trying to update your education!");
            }
        }

        public async Task<List<EducationDto>> GetEducationPageDtosByEmployeeIdAsync(Guid employeeId, int page)
        {
            try
            {
                IEnumerable<Education> educations = await educationRepository.GetEducationsPageByEmployeeIdAsync(employeeId, page);
                List<EducationDto> educationDtos = educations.Select(e => mapper.Map<EducationDto>(e)).ToList();
                return educationDtos;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetEducationCountByEmployeeIdAsync(Guid employeeId)
        {
            try
            {
                return await educationRepository.GetEducationCountByEmployeeIdAsync(employeeId);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
