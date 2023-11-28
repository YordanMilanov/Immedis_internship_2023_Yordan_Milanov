namespace HCMS.Web.ViewModels.Advert
{
    public class AdvertResultQueryModel : AdvertPageQueryModel
    {
        public AdvertResultQueryModel() : base()
        {
            this.Items = new List<AdvertViewModel>();
        }

        public IEnumerable<AdvertViewModel> Items { get; set; }
    }
}
