﻿namespace HCMS.Web.ViewModels.WorkRecord
{
    public class WorkRecordViewModel
    {

        public WorkRecordViewModel()
        {
            CompanyName = "Unknown";
            Department = "Unknown";
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Position { get; set; } = null!;
        public string? Department { get; set; }
        public string? CompanyName { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
