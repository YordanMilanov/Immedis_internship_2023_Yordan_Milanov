using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.Advert;

namespace HCMS.Web.ViewModels.Advert
{
    public class AdvertFormModel
    {
        [Required]
        [StringLength(PositionMaxLength, MinimumLength = PositionMinLength)]
        public string Position { get; set; } = null!;
       
        [Required]
        [StringLength(DepartmentMaxLength, MinimumLength = DepartmentMinLength)]
        public string Department { get; set; } = null!;

        [Required]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        [Display(Name = "Remote option")]
        public bool RemoteOption { get; set; }

        [Required]
        [Range(0,Double.MaxValue)]
        public decimal Salary { get; set; }
       
        [Required]
        public DateTime AddDate { get; set; }
        public Guid CompanyId { get; set; }
    }
}
