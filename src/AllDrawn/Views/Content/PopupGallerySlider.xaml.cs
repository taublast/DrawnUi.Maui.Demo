using AppoMobi.Maui.DrawnUi.Demo.Interfaces;

namespace AppoMobi.Maui.DrawnUi.Demo.Views;

public partial class PopupGallerySlider
{
    public PopupGallerySlider(IFullscreenGalleryManager vm)
    {
        BindingContext = vm;

        InitializeComponent();
    }


}