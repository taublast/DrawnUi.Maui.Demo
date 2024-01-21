namespace AppoMobi.Maui.DrawnUi.Demo.Views;

public partial class ScreenCarousel
{
    public ScreenCarousel()
    {
        InitializeComponent();

        var vm = App.Instance.Services.GetService<SimplePageViewModel>();
        vm.Title = "Carousel";
        BindingContext = vm;
    }



}