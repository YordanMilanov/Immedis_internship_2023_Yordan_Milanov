namespace HCMS.Web.ViewModels.Company
{
    public class CompanyViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string IndustryField { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
