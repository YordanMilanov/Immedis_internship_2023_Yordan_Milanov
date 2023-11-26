namespace HCMS.Web.ViewModels.Advert
{
    public class AdvertViewModel
    {
        public string Position { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public bool RemoteOption { get; set; }
        public decimal Salary { get; set; }
        public DateTime AddDate { get; set; }
        public Guid CompanyId { get; set; }
    }
}
