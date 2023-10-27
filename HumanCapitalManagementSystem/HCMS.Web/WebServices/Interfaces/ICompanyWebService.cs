namespace HCMS.Web.WebServices.Interfaces
{
    public interface ICompanyWebService
    {
        Task<IEnumerable<string>> GetAllCompanyNamesFromApiAsync();
    }
}
