namespace HCMS.Services.ServiceModels.BaseClasses
{
    public class QueryDtoResult<T> : QueryDto
    {

        public int TotalItems { get; set; }

        public IEnumerable<T> Items { get; set; } = null!;
    }
}
