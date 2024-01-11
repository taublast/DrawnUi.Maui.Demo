namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls;

public class MainTabLayout : SkiaLayout, IInsideViewport
{
    bool vmSet;

    public void OnAppearing()
    {
        if (!vmSet)
        {
            vmSet = true;
            var vm = App.Instance.Services.GetService<SomeTabsViewModel>();
            BindingContext = vm;
            vm.CommandRefreshSmallData.Execute(null);
        }
    }

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




}