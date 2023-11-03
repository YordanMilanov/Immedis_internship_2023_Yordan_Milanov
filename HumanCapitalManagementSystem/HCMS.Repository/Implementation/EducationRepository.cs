using AutoMapper;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository.Implementation
{
    internal class EducationRepository : IEducationRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public EducationRepository(ApplicationDbContext dbContext, IMapper mapper)
        {

            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task AddEducationAsync(Education education)
        {
            education.Id = Guid.NewGuid();
            try
            {
                if(education.Location != null)
                {
                    Location location = new Location
                    {
                        Address = education.Location.Address,
                        State = education.Location.State,
                        Country = education.Location.Country,
                        OwnerId = education.Id,
                        OwnerType = education.GetType().Name
                    };
                    education.Location = location;
                    education.LocationId = location.Id;
                }


                await dbContext.Educations.AddAsync(education);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Unexpected error occurred!");
            }
        }

        public async Task<Education> GetEducationByIdAsync(Guid id)
        {
            try
            {
                return await dbContext.Educations.Include(e => e.Location).FirstAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                throw new Exception($"Education with id: {id} was not found!");
            }
        }

        public async Task UpdateEducationAsync(Education educationInfo)
        {
            try
            {
                Education education = await dbContext.Educations.Include(e => e.Location).FirstAsync(e => e.Id == educationInfo.Id);
                education.University = educationInfo.University;
                education.FieldOfEducation = educationInfo.FieldOfEducation;
                education.Degree = educationInfo.Degree;
                education.Grade = educationInfo.Grade;
                education.StartDate = educationInfo.StartDate;
                education.EndDate = educationInfo.EndDate;
                if(education.LocationId != Guid.Empty)
                {
                    education.Location!.Country = educationInfo.Location!.Address;
                    education.Location.State = educationInfo.Location.State;
                    education.Location.Country = educationInfo.Location.Country;
                } else
                {
                    Location location = new Location
                    {
                        Address = educationInfo.Location!.Address,
                        State = educationInfo.Location.State,
                        Country = educationInfo.Location.Country,
                        OwnerId = education.Id,
                        OwnerType = education.GetType().ToString()
                    };
                    education.Location = location;
                    education.LocationId = location.Id;
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Unexpected error occurred while updating education!");
            }
        }

        public async Task<IEnumerable<Education>> GetEducationsPageByEmployeeIdAsync(Guid employeeId, int page)
        {
            
            try
            {
                return await dbContext.Educations.Include(e => e.Location).Where(e => e.EmployeeId == employeeId).Skip((page - 1) * 3).Take(3).ToListAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetEducationCountByEmployeeIdAsync(Guid employeeId)
        {
            try
            {
                return await dbContext.Educations.Where(e => e.EmployeeId == employeeId).CountAsync();
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}
