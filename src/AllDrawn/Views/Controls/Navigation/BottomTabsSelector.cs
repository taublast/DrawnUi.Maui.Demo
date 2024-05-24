using DrawnUi.Maui.Controls;



namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls.Navigation;

public class BottomTabsSelector : SkiaTabsSelector
{
    public BottomTabsSelector()
    {
        TabType = typeof(SkiaSvg);
    }

    public override ISkiaGestureListener ProcessGestures(SkiaGesturesParameters args, GestureEventProcessingInfo apply)
    {

        if (args.Type != TouchActionResult.Tapped)
        {
            return this; //pass taps only
        }

        return base.ProcessGestures(args, apply);
    }

    async Task AnimatePulseIcon(SkiaControl icon)
    {
        await icon.ScaleToAsync(0.8, 0.8, 50, Easing.CubicOut);
        await icon.ScaleToAsync(1.2, 1.2, 100, Easing.CubicInOut);
        await icon.ScaleToAsync(1.0, 1.0, 100, Easing.SpringOut);
    }

    public void UpdateIcons()
    {
        //for (int i = 0; i < SelectableTabs.Count; i++)
        //{
        //	if (i == SelectedIndex)
        //	{
        //		SelectableTabs[i].VIew.Opacity = 1;
        //	}
        //	else
        //	{
        //		SelectableTabs[i].VIew.Opacity = 0.66;
        //	}
        //}
    }

    public override async Task OnTabSelectionChanged(bool tabsChanged, int index)
    {
        if (SelectedIndex >= 0 && !tabsChanged)
        {
            await AnimatePulseIcon(SelectableTabs[index].VIew);

            UpdateIcons();
        }

        await base.OnTabSelectionChanged(tabsChanged, index);
    }

    protected override void OnLayoutChanged()
    {
        base.OnLayoutChanged();

        UpdateIcons();
    }

}

