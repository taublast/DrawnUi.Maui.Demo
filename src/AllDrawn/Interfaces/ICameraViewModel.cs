using DrawnUi.Maui.Camera;
using DrawnUi.Maui.Controls;

namespace AppoMobi.Maui.DrawnUi.Demo.Interfaces;

public interface ICameraViewModel : IVisibilityAware, IDisposable
{
    void AttachCamera(SkiaCamera camera);
}