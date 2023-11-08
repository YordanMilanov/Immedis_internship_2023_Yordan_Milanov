using HCMS.Common;

namespace HCMS.Services.ServiceModels.BaseClasses
{
    public class QueryDto
    {
        public QueryDto()
        {
            this.CurrentPage = 1;
            this.ItemsPerPage = 3;
        }

        public string? SearchString { get; set; }
        public OrderPageEnum OrderPageEnum { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
