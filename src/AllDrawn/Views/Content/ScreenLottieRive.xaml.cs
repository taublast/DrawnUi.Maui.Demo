namespace AppoMobi.Maui.DrawnUi.Demo.Views.Content;

public partial class ScreenLottieRive : SkiaLayout
{
    public ScreenLottieRive()
    {
        var vm = App.Instance.Services.GetService<SimplePageViewModel>();
        vm.Title = "Consume Pre-made Animations";
        BindingContext = vm;

        InitializeComponent();
    }

    private void Editor_TextSubmitted(object sender, string e)
    {

    }

}