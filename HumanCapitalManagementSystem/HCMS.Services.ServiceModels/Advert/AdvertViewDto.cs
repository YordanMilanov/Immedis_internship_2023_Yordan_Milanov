using HCMS.Common.Structures;

namespace HCMS.Services.ServiceModels.Advert
{
    public class AdvertViewDto : AdvertAddDto
    {
        public LocationStruct Location { get; set; }
        public Name CompanyName { get; set; }
    }
}
