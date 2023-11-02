namespace AppoMobi.Maui.DrawnUi.Demo.Views.Content;

public partial class ScreenControls : ISkiaLayout
{
    public ScreenControls()
    {
        var vm = App.Instance.Services.GetService<SimplePageViewModel>();
        vm.Title = "Some Controls";
        BindingContext = vm;

        InitializeComponent();
    }

}