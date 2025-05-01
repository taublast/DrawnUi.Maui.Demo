using System.Diagnostics;
using AppoMobi.Maui.DrawnUi.Demo.Interfaces;

namespace AppoMobi.Maui.DrawnUi.Demo.Views;

public partial class PopupGallerySlider
{
    public PopupGallerySlider(IFullscreenGalleryManager vm)
    {
        BindingContext = vm;

        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is IFullscreenGalleryManager ctx)
        {
            var check = ctx.SelectedGalleryIndex;

            Debug.WriteLine($"[GALLERY] Index {check}");
        }
    }
}