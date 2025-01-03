namespace AppoMobi.Maui.DrawnUi.Demo.Views;

public partial class TabWithTopTabs
{
    public TabWithTopTabs()
    {
        InitializeComponent();
    }


    private void TabWithTopTabs_OnWasFirstTimeDrawn(object sender, EventArgs e)
    {
        Tasks.StartDelayed(TimeSpan.FromMilliseconds(500), async () =>
        {
            Picker.IsVisible = true;
            await Picker.FadeToAsync(1);
        });
    }
}