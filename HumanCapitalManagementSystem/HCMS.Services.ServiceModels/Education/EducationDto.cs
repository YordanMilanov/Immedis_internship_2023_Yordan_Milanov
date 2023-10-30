using HCMS.Common.Structures;

namespace HCMS.Services.ServiceModels.Education
{
    public class EducationDto
    {
        public Guid Id { get; set; }

        public string University { get; set; } = null!;

        public string Degree { get; set; } = null!;

        public string FieldOfEducation { get; set; } = null!;

        public decimal Grade { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid EmployeeId { get; set; }

        public LocationStruct Location { get; set; }
    }
}
