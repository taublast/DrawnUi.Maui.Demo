namespace AppoMobi.Maui.DrawnUi.Demo.Views;

public partial class ScreenVarious : SkiaLayout
{
    public ScreenVarious()
    {
        var vm = App.Instance.Services.GetService<SimplePageViewModel>();
        vm.Title = "Maui Control On A Canvas";
        BindingContext = vm;

        InitializeComponent();
    }

    private void Editor_TextSubmitted(object sender, string e)
    {

    }

}