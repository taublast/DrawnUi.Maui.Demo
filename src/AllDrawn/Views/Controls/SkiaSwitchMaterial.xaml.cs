namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls;

public partial class SkiaSwitchMaterial : SkiaSwitch
{
    public SkiaSwitchMaterial()
    {
        InitializeComponent();
    }

    protected void OnTapped(object sender, TouchActionEventArgs e)
    {
        IsToggled = !IsToggled;
    }

}