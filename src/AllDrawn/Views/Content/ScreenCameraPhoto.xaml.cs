using DrawnUi.Maui.Controls;
using DrawnUi.Maui;

namespace AppoMobi.Maui.DrawnUi.Demo.Views;

public partial class ScreenCameraPhoto
{

#if DEBUG

    public ScreenCameraPhoto()
    {
        _vm = App.Instance.Services.GetService<TakePictureViewModel>();

        BindingContext = _vm;

        InitializeComponent();
    }

#endif

    private readonly TakePictureViewModel _vm;


    public ScreenCameraPhoto(TakePictureViewModel vm)
    {
        _vm = vm;

        BindingContext = _vm;

        InitializeComponent();
    }


    private void TappedSwitchCamera(object sender, TouchActionEventArgs e)
    {
        if (CameraControl.IsOn)
        {
            CameraControl.Facing = CameraControl.Facing == CameraPosition.Selfie ? CameraPosition.Default : CameraPosition.Selfie;
        }
    }

    private void TappedTurnCamera(object sender, TouchActionEventArgs e)
    {
        if (CameraControl.State == CameraState.On)
        {
            CameraControl.IsOn = false;
        }
        else
        {
            CameraControl.IsOn = true;
        }
    }



    private void TappedCycleEffects(object sender, TouchActionEventArgs e)
    {
        var available = new List<SkiaImageEffect>()
        {
            SkiaImageEffect.None,
            SkiaImageEffect.Sepia,
            SkiaImageEffect.BlackAndWhite,
            SkiaImageEffect.Pastel,
        };

        var current = CameraControl.Effect;
        var index = available.IndexOf(current);
        index++;
        if (index >= available.Count)
        {
            index = 0;
        }
        CameraControl.Effect = available[index];
    }

    private async void TappedTakePicture(object sender, TouchActionEventArgs e)
    {
        if (CameraControl.State == CameraState.On && !CameraControl.IsBusy)
        {
            CameraControl.FlashScreen(Color.Parse("#EEFFFFFF"));
            await CameraControl.TakePicture().ConfigureAwait(false);
        }
    }

    private void TappedResume(object sender, TouchActionEventArgs e)
    {
        CameraControl.IsOn = true;
    }

    float step = 0.2f;

    private void Tapped_ZoomOut(object sender, TouchActionEventArgs e)
    {
        CameraControl.Zoom -= step;
    }

    private void Tapped_ZoomIn(object sender, TouchActionEventArgs e)
    {
        CameraControl.Zoom += step;
    }

    private void OnZoomed(object sender, ZoomEventArgs e)
    {
        CameraControl.Zoom = e.Value;
    }
}