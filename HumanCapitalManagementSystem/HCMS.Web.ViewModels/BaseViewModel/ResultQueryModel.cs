namespace HCMS.Web.ViewModels.BaseViewModel
{
    public class ResultQueryModel<T> : PageQueryModel
    {
        public ResultQueryModel() : base()
        {
            this.Items = new List<T>();
        }

        public IEnumerable<T> Items { get; set; }
    }
}
