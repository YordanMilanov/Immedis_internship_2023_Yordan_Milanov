namespace HCMS.Services.ServiceModels.Application
{
    public class ApplicationDto
    {
        public string CoverLetter { get; set; } = null!;
        public DateTime AddDate { get; set; }
        public Guid FromEmployeeId{ get; set; }
        public Guid AdvertId { get; set; }
    }
}
