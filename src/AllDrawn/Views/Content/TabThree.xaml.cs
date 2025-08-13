using DrawnUi.Camera;

namespace AppoMobi.Maui.DrawnUi.Demo.Views;

public partial class TabButtons
{
	public TabButtons()
	{
		InitializeComponent();
	}

    private void TappedPreview(object sender, ControlTappedEventArgs e)
    {
        if (BindingContext is MainPageViewModel vm)
        {
            SkiaCamera.OpenFileInGallery(vm.LastSavedPhoto);
        }
    }
}