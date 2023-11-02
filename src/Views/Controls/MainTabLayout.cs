namespace AppoMobi.Maui.DrawnUi.Demo.Views.Content;

public class MainTabLayout : SkiaLayout, IInsideViewport
{
	bool vmSet;

	protected override void OnBindingContextChanged()
	{
		//avoid parent tabs binding context
		if (BindingContext != null && !(BindingContext is SomeTabsViewModel))
		{
			BindingContext = null;
			return;
		}

		base.OnBindingContextChanged();
	}

	public override void OnLoaded()
	{
		base.OnLoaded();

		if (!vmSet)
		{
			vmSet = true;
			var vm = App.Instance.Services.GetService<SomeTabsViewModel>();
			BindingContext = vm;
			vm.RefreshCommandData.Execute(null);
		}

	}


}