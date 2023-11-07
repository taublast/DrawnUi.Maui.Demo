using AppoMobi.Maui.DrawnUi.Controls;

namespace AppoMobi.Maui.DrawnUi.Demo.Interfaces;

public interface ICameraViewModel : IVisibilityAware, IDisposable
{
    void AttachCamera(SkiaCamera camera);
}