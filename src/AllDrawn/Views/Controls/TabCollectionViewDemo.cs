namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls;

public class TabCollectionViewDemo : SkiaLayout
{
    bool vmSet;

    public override void OnAppearing()
    {
        base.OnAppearing();

        if (!vmSet)
        {
            vmSet = true;
            var vm = App.Instance.Services.GetService<ScrollingCellsViewModel>();
            BindingContext = vm;
            vm.CommandRefreshData.Execute(null);
        }
    }

}