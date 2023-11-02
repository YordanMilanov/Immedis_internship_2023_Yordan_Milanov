namespace HCMS.Web.ViewModels.Education
{
    public class EducationPageModel
    {
        public EducationPageModel()
        {
            this.Page = 1;
            this.Educations = new List<EducationViewModel>();
        }

        public int Page { get; set; }
        public int TotalEducations { get; set; }
        public List<EducationViewModel> Educations { get; set; } = null!;
    }
}
