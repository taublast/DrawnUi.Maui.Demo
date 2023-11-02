

using AppoMobi.Maui.DrawnUi.Controls;

namespace AppoMobi.Maui.DrawnUi.Demo.Views.Navigation;

public class BottomTabsSelector : SkiaTabsSelector
{

	public BottomTabsSelector()
	{
		TabType = typeof(SkiaSvg);
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

