using HCMS.Common;
using System.ComponentModel;


namespace HCMS.Web.ViewModels.BaseViewModel
{
    public class PageQueryModel
    {
        public PageQueryModel()
        {
            this.CurrentPage = 1;
            this.ItemsPerPage = 3;
        }

        public PageQueryModel(int currentPage, int itemsPerPage)
        {
            this.CurrentPage = currentPage;
            this.ItemsPerPage = itemsPerPage;
        }

        [DisplayName("Search by word")]
        public string? SearchString { get; set; }

        [DisplayName("Sort by")]
        public OrderPageEnum OrderPageEnum { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }

        [DisplayName("Per page")]
        public int ItemsPerPage { get; set; }
    }
}
