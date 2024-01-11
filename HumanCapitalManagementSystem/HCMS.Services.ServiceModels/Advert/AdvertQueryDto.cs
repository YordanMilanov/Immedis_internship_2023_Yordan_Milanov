using HCMS.Services.ServiceModels.BaseClasses;

namespace HCMS.Services.ServiceModels.Advert
{
    public class AdvertQueryDto : QueryDto
    {
        public bool? RemoteOption { get; set; }

        public string? CompanyId { get; set; }
    }
}
