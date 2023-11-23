using HCMS.Common.Structures;

namespace HCMS.Services.ServiceModels.Recommendation
{
    public class RecommendationDto
    {
        public Guid Id { get; set; }
        public Description Description { get; set; }
        public Email EmployeeEmail { get; set; }
        public Name CompanyName { get; set; }
        public Guid RecommenderId { get; set; }
    }
}
