using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HCMS.Web.ViewModels.Education
{
    public class EducationViewModel
    {
        public Guid Id { get; set; }
        public string University { get; set; } = null!;
        public string FieldOfEducation { get; set; } = null!;
        public string Degree { get; set; } = null!;
        public decimal Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Address { get; set; }
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}
