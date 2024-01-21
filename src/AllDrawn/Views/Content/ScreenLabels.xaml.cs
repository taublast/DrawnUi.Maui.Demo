namespace AppoMobi.Maui.DrawnUi.Demo.Views;

public partial class ScreenLabels : SkiaLayout
{
    public ScreenLabels()
    {
        var vm = App.Instance.Services.GetService<SimplePageViewModel>();
        vm.Title = "SkiaScroll";
        BindingContext = vm;

        InitializeComponent();
    }
}