namespace AppoMobi.Maui.DrawnUi.Demo.Views.Content;

public partial class ScreenCarousel
{
    public ScreenCarousel()
    {
        var vm = App.Instance.Services.GetService<SimplePageViewModel>();
        vm.Title = "SkiaCarousel";
        BindingContext = vm;

        InitializeComponent();
    }


}