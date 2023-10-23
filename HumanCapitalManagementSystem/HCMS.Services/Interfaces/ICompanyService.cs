using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMS.Web.ViewModels.Company;

namespace HCMS.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<string>> GetAllCompanyNames();

        Task<CompanyViewModel> GetCompanyByUserId(Guid id);
    }
}
