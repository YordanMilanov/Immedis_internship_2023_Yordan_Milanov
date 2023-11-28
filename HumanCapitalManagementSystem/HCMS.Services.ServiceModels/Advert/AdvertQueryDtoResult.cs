namespace HCMS.Services.ServiceModels.Advert
{
    public class AdvertQueryDtoResult : AdvertQueryDto
    {

        public int TotalItems { get; set; }

        public IEnumerable<AdvertViewDto> Items { get; set; } = null!;
    }
}
