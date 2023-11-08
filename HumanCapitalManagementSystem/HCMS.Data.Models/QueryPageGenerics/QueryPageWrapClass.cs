using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Data.Models.QueryPageGenerics
{
    [NotMapped]
    public class QueryPageWrapClass<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; } = null!;
    }
}
