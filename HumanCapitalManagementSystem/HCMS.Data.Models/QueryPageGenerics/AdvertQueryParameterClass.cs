using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Data.Models.QueryPageGenerics
{
    [NotMapped]
    public class AdvertQueryParameterClass : QueryParameterClass
    {
        public bool RemoteOption { get; set; }
    }
}
