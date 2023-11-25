using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace HCMS.Repository.Implementation
{
    internal class CompanyRepository : ICompanyRepository
    {

        private readonly ApplicationDbContext dbContext;

        public CompanyRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Company> GetByIdAsync(Guid id)
        {
            try
            {
                return await dbContext.Companies
                    .Include(c => c.Location)
                    .FirstAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                            .ThenInclude(c => c!.Location)
                            .Select(e => e.Company)
                            .FirstAsync();
                return company!;
            }
            catch (Exception)
            {
                throw new Exception("Company not found");
            }

        }

        public async Task<QueryPageWrapClass<Company>> GetCurrentPageAndTotalCountAsync(QueryParameterClass parameters)
        {
            try
            {
                IQueryable<Company> query = dbContext.Companies.Include(c => c.Location);

                //check the search string
                if (parameters.SearchString != null)
                {
                    query = query.Where(company =>
                    company.Name.Contains(parameters.SearchString!) ||
                    company.IndustryField.Contains(parameters.SearchString!) ||
                    company.Description.Contains(parameters.SearchString!) ||
                    company.Location!.Country.Contains(parameters.SearchString!) ||
                    company.Location!.State.Contains(parameters.SearchString!) ||
                    company.Location!.Address.Contains(parameters.SearchString!));
                }

                int totalCount = await query.CountAsync();

                //Pagination
                int workRecordsCountToSkip = (parameters.CurrentPage - 1) * parameters.ItemsPerPage;
                query = query.Skip(workRecordsCountToSkip).Take(parameters.ItemsPerPage);

                List<Company> companies = await query.ToListAsync();
                return new QueryPageWrapClass<Company>()
                {
                    TotalItems = totalCount,
                    Items = companies,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            };
        }

        public async Task<bool> CompanyExistsByNameAsync(string name)
        {
            return await this.dbContext.Companies.AnyAsync(c => c.Name == name);
        }

        public async Task AddCompanyAsync(Company model)
        {
            try
            {
                await this.dbContext.Companies.AddAsync(model);
                await this.dbContext.SaveChangesAsync();
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EditCompanyAsync(Company model)
        {
            try
            {
                if(await this.dbContext.Companies.AnyAsync(c => c.Name == model.Name && c.Id != model.Id))
                {
                    throw new Exception("Company name is already used");
                }

                Company company = await this.dbContext.Companies.Include(c => c.Location).FirstAsync(c => c.Id == model.Id);
                company.Name = model.Name;
                company.IndustryField = model.IndustryField;
                company.Description = model.Description;
                company.Location!.Address = model.Location!.Address;
                company.Location.State = model.Location.State;
                company.Location.Country = model.Location.Country;
                await this.dbContext.SaveChangesAsync();
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       public async Task<string> GetCompanyNameById(Guid id)
        {
            try
            {
                return await this.dbContext.Companies
                         .Where(c => c.Id == id)
                         .Select(c => c.Name)
                         .FirstAsync();
            } catch(Exception)
            {
                throw new Exception("Company name was not found!");
            }
        }
    }
}
