using HCMS.Data.Models;
using HCMS.Web.ViewModels.User;

namespace HCMS.Repository.Interfaces
{
    public interface ILocationRepository
    {
        Task<Location?> GetLocationByCountryStateAddressAsync(string country, string state, string address);

    }
}
