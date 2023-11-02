namespace AppoMobi.Maui.DrawnUi.Demo.Views.Content;

public class StackUsers : SkiaLayout
{
	private ScrollingCellsViewModel _vm;

	public override void OnParentChanged(IDrawnBase newvalue, IDrawnBase oldvalue)
	{
		base.OnParentChanged(newvalue, oldvalue);

		if (_vm == null)
		{
			_vm = App.Instance.Services.GetService<ScrollingCellsViewModel>();
		}

		BindingContext = _vm;
	}
}