using HCMS.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Data.Models.QueryPageGenerics
{
    [NotMapped]
    public class QueryParameterClass
    {
        public string? SearchString { get; set; }
        public OrderPageEnum OrderPageEnum { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
