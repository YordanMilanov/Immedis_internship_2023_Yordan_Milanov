using Newtonsoft.Json;
using HCMS.Web.WebServices.Interfaces;

namespace HCMS.Web.WebServices
{
    public class CompanyWebService : ICompanyWebService
    {
        private readonly HttpClient httpClient;

        public CompanyWebService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("WebApi");
        }

        public async Task<IEnumerable<string>> GetAllCompanyNamesFromApiAsync()
        {
            //all companies select
            string allCompanyUrl = "api/company/AllCompanies";
            HttpResponseMessage response = await httpClient.GetAsync(allCompanyUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                IEnumerable<string> companies = JsonConvert.DeserializeObject<IEnumerable<string>>(jsonContent)!;
                return companies;
            }
            return Enumerable.Empty<string>();
        }
    }
}
