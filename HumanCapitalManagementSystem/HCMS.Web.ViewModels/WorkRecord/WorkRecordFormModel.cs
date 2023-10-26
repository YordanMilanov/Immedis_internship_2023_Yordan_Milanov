using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HCMS.Common.CustomValidationAnnotation;
using static HCMS.Common.DataModelConstants.WorkRecord;

namespace HCMS.Web.ViewModels.WorkRecord
{
    public class WorkRecordFormModel
    {

        [Required]
        [StringLength(PositionMaxLength, MinimumLength = PositionMinLength, ErrorMessage = "Position name must be a text between {2} and {1} symbols long")]
        public string Position { get; set; } = null!;

        [StringLength(DepartmentMaxLength, MinimumLength = DepartmentMinLength, ErrorMessage = "Department name must be a text between {2} and {1} symbols long")]
        public string? Department { get; set; }

        [Display(Name = "Company name")]
        [StringLength(DepartmentMaxLength, MinimumLength = DepartmentMinLength, ErrorMessage = "Company name must be a text between {2} and {1} symbols long")]
        public string? CompanyName { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Value must be a positive number.")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start date")]
        [DatePastOrPresent(ErrorMessage = "Date must be in the past.")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public DateTime AddDate { get; set; } = DateTime.Now;
    }
}
