namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls;



public partial class SkiaSwitchCupertino : SkiaSwitch
{
    public SkiaSwitchCupertino()
    {
        InitializeComponent();
    }

    protected void OnTapped(object sender, SkiaGesturesParameters skiaGesturesParameters)
    {
        IsToggled = !IsToggled;
    }

}