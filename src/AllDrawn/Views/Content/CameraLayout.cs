using AppoMobi.Maui.DrawnUi.Demo.Interfaces;
using DrawnUi.Maui.Camera;
using DrawnUi.Maui.Controls;


namespace AppoMobi.Maui.DrawnUi.Demo.Views;

/// <summary>
/// We are subclassing just to use overrides because inside ElementAdapter we cannot
/// </summary>
public class CameraLayout : SkiaLayout
{
    public override void OnDisposing()
    {
        base.OnDisposing();

        if (BindingContext is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    public override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ICameraViewModel vm)
        {
            _camera = this.FindView<SkiaCamera>("Camera");

            vm.AttachCamera(_camera);

            vm.OnAppearing();
        }

    }

    SkiaCamera _camera;

    volatile bool _sVisible;

    public override void OnAppeared()
    {
        _sVisible = true;

        base.OnAppeared();

        if (BindingContext is ICameraViewModel vm)
        {
            vm.OnAppeared();
        }

        if (_camera != null)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (_sVisible && App.Shell.ModalStack.Count == 1) //we are alone as modal
                {
                    _camera.IsOn = true;
                }
            });
        }
    }

    public override void OnDisappeared()
    {
        _sVisible = false;

        base.OnDisappeared();

        if (_camera != null && !_camera.IsDisposed)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _camera.IsOn = false;
            });
        }
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();

        if (_camera != null)
        {
            _camera.IsOn = false;
        }

        if (BindingContext is ICameraViewModel vm)
        {
            vm.OnDisappeared();
        }
    }

}