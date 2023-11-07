namespace AppoMobi.Maui.DrawnUi.Demo.Views.Content;

public class TabCollectionViewDemo : SkiaLayout, IInsideViewport
{
	bool vmSet;

	public void OnAppearing()
	{
		if (!vmSet)
		{
			vmSet = true;
			var vm = App.Instance.Services.GetService<ScrollingCellsViewModel>();
			BindingContext = vm;
			vm.CommandRefreshData.Execute(null);
		}
	}

	public override ScaledSize Measure(float widthConstraint, float heightConstraint, float scale)
	{
		var check = base.Measure(widthConstraint, heightConstraint, scale);
		return check;
	}

}