using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMS.Web.ViewModels.WorkRecord.Enum;

namespace HCMS.Web.ViewModels.WorkRecord
{
    public class WorkRecordAllQueryModel
    {
        public WorkRecordAllQueryModel()
        {
            this.CurrentPage = 1;
            this.WorkRecordsPerPage = 3;

            this.WorkRecords = new HashSet<WorkRecordViewModel>();
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
