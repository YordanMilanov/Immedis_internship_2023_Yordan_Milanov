using HCMS.Common.Structures;
using HCMS.Services.ServiceModels.Employee;

namespace HCMS.Services.ServiceModels.Recommendation
{
    public class RecommendationDto
    {
        public Guid Id { get; set; }
        public Description Description { get; set; }
        public DateTime RecommendDate { get; set; }
        public Email EmployeeEmail { get; set; }
        public EmployeeDto? RecommendedEmployee { get; set; }
        public Name CompanyName { get; set; }
        public Guid RecommenderId { get; set; }
    }
}
