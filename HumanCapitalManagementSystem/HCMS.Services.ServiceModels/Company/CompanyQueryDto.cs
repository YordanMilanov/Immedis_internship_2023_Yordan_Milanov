namespace HCMS.Services.ServiceModels.Company
{
    public class CompanyQueryDto
    {
        public string? SearchString { get; set; }

        public int CurrentPage { get; set; }

        public int TotalCompanies { get; set; }

        public int CompaniesPerPage { get; set; }

        public IEnumerable<CompanyDto> Companies { get; set; } = null!;
    }
}
