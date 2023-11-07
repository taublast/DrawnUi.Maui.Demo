namespace AppoMobi.Maui.DrawnUi.Demo.Views.Content;

public partial class ScreenWithDrawer : SkiaLayout
{
    public ScreenWithDrawer()
    {
        var vm = App.Instance.Services.GetService<SimplePageViewModel>();
        vm.Title = "Try Drawer";
        BindingContext = vm;

        InitializeComponent();
    }
}