namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls;

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


}