using AppoMobi.Maui.DrawnUi.Controls;
using AppoMobi.Maui.DrawnUi.Drawn.Infrastructure.Interfaces;

namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls.Navigation;

public class BottomTabsSelector : SkiaTabsSelector
{
    public BottomTabsSelector()
    {
        TabType = typeof(SkiaSvg);
    }

    public override ISkiaGestureListener ProcessGestures(TouchActionType type, TouchActionEventArgs args, TouchActionResult touchAction,
        SKPoint childOffset, SKPoint childOffsetDirect, ISkiaGestureListener alreadyConsumed)
    {

        if (touchAction != TouchActionResult.Tapped)
        {
            return this; //pass taps only
        }

        return base.ProcessGestures(type, args, touchAction, childOffset, childOffsetDirect, alreadyConsumed);
    }

    async void AnimatePulseIcon(SkiaControl icon)
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

    public override void OnTabSelectionChanged(bool tabsChanged)
    {
        if (SelectedIndex >= 0 && !tabsChanged)
        {
            AnimatePulseIcon(SelectableTabs[SelectedIndex].VIew);

            UpdateIcons();
        }

        base.OnTabSelectionChanged(tabsChanged);
    }

    protected override void OnLayoutChanged()
    {
        base.OnLayoutChanged();

        UpdateIcons();
    }

}

