namespace AppoMobi.Maui.DrawnUi.Demo.Views;

public partial class ScreenAnims : SkiaLayout
{
    public ScreenAnims()
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