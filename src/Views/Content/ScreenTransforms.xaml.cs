namespace AppoMobi.Maui.DrawnUi.Demo.Views.Content;

public partial class ScreenTransforms : SkiaLayout
{
    public ScreenTransforms()
    {
        var vm = App.Instance.Services.GetService<SimplePageViewModel>();
        vm.Title = "Transforms";
        BindingContext = vm;

        InitializeComponent();
    }


}