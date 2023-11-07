﻿using HCMS.Common;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository.Implementation
{
    internal class CompanyRepository : ICompanyRepository
    {

        private readonly ApplicationDbContext dbContext;

        public CompanyRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Company?> GetByIdAsync(Guid id)
        {
            try
            {
                return await dbContext.Companies
                    .Include(c => c.Location)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<Company?> GetByNameAsync(string name)
        {
            try
            {
                return await dbContext.Companies
                    .Include(c => c.Location)
                    .FirstOrDefaultAsync(c => c.Name == name);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            try
            {
                return await dbContext.Companies
                    .Include(c => c.Location)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<IEnumerable<string>> GetAllCompanyNamesAsync()
        {
            try
            {
                return await dbContext.Companies
                    .Select(c => c.Name)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        public async Task<Company> GetCompanyByNameAsync(string name)
        {
            try
            {
                return await dbContext.Companies.FirstAsync(c => c.Name == name);
            }
            catch (Exception)
            {
                throw new Exception("Company not found!");
            }
        }

        public async Task<Company> GetCompanyByEmployeeIdAsync(Guid employeeId)
        {
            try
            {
                Company? company = await dbContext.Employees
                            .Where(e => e.Id == employeeId)
                            .Include(e => e.Company)
                            .Select(e => e.Company)
                            .FirstAsync();
                return company!;
            }
            catch (Exception)
            {
                throw new Exception("Company not found");
            }

        }

        public async Task<(int, IEnumerable<Company>)> GetCurrentPageAndTotalCountAsync(int currentPage, string? searchString, int companiesPerPage)
        {
            try
            {
                IQueryable<Company> query = dbContext.Companies.Include(c => c.Location);

                //check the search string
                if (searchString != null)
                {
                    query = query.Where(company =>
                    company.Name.Contains(searchString!) ||
                    company.IndustryField.Contains(searchString!) ||
                    company.Description.Contains(searchString!) ||
                    company.Location!.Country.Contains(searchString!) ||
                    company.Location!.State.Contains(searchString!) ||
                    company.Location!.Address.Contains(searchString!));
                }

                int totalCount = await query.CountAsync();

                //Pagination
                int workRecordsCountToSkip = (currentPage - 1) * companiesPerPage;
                query = query.Skip(workRecordsCountToSkip).Take(companiesPerPage);

                List<Company> companies = await query.ToListAsync();
                return (totalCount, companies);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            };
        }
    }
}
