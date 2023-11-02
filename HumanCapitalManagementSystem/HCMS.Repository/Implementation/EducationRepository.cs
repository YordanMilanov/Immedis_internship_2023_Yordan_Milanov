using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository.Implementation
{
    internal class EducationRepository : IEducationRepository
    {
        private readonly ApplicationDbContext dbContext;

        public EducationRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddEducationAsync(Education education)
        {
            try
            {
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
                return await dbContext.Educations.FirstAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                throw new Exception($"Education with id: {id} was not found!");
            }
        }

        public async Task UpdateEducationAsync(Education education)
        {
            try
            {
                dbContext.Educations.Update(education);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Unexpected error occurred while updating education!");
            }

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Education>> GetAllEducationsByEmployeeIdAsync(Guid employeeId)
        {
            
            try
            {
                return await dbContext.Educations.Include(e => e.Location).Where(e => e.EmployeeId == employeeId).ToListAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
