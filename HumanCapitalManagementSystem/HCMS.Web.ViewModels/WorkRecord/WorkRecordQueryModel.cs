using HCMS.Common;
using System.ComponentModel;

namespace HCMS.Web.ViewModels.WorkRecord
{
    public class WorkRecordQueryModel
    {
        public WorkRecordQueryModel()
        {
            this.CurrentPage = 1;
            this.WorkRecordsPerPage = 3;

            this.WorkRecords = new List<WorkRecordViewModel>();
        }

        [DisplayName("Search by word")]
        public string? SearchString { get; set; }

        [DisplayName("Sort by")]
        public OrderPageEnum OrderPageEnum { get; set; }

        public int CurrentPage { get; set; }

        public int TotalWorkRecords { get; set; }
        
        [DisplayName("Per page")]
        public int WorkRecordsPerPage { get; set; }

        public IEnumerable<WorkRecordViewModel> WorkRecords { get; set; }
    }
}
