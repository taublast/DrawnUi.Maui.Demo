namespace AppoMobi.Maui.DrawnUi.Demo.ViewModels
{
    public class SimpleItemViewModel : IHasBanner, IHasStringTitle, IHasIntId
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Banner { get; set; }

        public bool BannerPreloadOrdered { get; set; }
    }


}
