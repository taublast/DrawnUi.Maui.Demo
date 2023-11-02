
using System.Windows.Input;

namespace AppoMobi.Maui.DrawnUi.Demo.Interfaces;


public interface IFullscreenGalleryManager
{
    public ICommand CommandCloseGallery { get; }
    public ICommand CommandPressingImage { get; }
    public int SelectedGalleryIndex { get; set; }
    public IList<string> GalleryItems { get; }
}