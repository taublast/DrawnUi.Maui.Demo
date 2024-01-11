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

    public override void OnParentChanged(IDrawnBase newvalue, IDrawnBase oldvalue)
    {
        Console.WriteLine($"[OnParentChanged] ScreenTransforms {newvalue}");
        base.OnParentChanged(newvalue, oldvalue);
    }
}