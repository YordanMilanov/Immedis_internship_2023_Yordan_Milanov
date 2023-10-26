using HCMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMS.Services.ServiceModels.WorkRecord
{
    public class WorkRecordDto
    {

        public Guid Id { get; set; }
        public string Position { get; set; } = null!;
        public string? Department { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime AddDate { get; set; } = DateTime.Now;


        //FKs
        public Guid CompanyId { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
