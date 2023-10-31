namespace HCMS.Services.ServiceModels.WorkRecord
{
    public class WorkRecordDto
    {

        public Guid Id { get; set; }
        public string Position { get; set; } = null!;
        public string? Department { get; set; }

        public string? CompanyName { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime AddDate { get; set; }

        public Guid EmployeeId { get; set; }
    }
}
