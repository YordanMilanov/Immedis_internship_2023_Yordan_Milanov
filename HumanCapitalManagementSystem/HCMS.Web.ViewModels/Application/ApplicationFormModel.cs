using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.Application;

namespace HCMS.Web.ViewModels.Application
{
    public class ApplicationFormModel
    {
        [MinLength(CoverLetterMinLength)]
        [Display(Name = "Cover letter")]
        public string CoverLetter { get; set; } = null!;
        public DateTime AddDate { get; set; }
        public Guid FromEmployeeId { get; set; }
        public Guid AdvertId { get; set; }
    }
}
