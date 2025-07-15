using DrawnUi;
using DrawnUi.Camera;
using DrawnUi.Controls;
using static Microsoft.Maui.ApplicationModel.Permissions;

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


    private void TappedSwitchCamera(object sender, ControlTappedEventArgs controlTappedEventArgs)
    {
        if (CameraControl.IsOn)
        {

            //just switch select front/back:
            //CameraControl.Facing = CameraControl.Facing == CameraPosition.Selfie ? CameraPosition.Default : CameraPosition.Selfie;
            
            //OR manual select:
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var cameras = await CameraControl.GetAvailableCamerasAsync();

                // Create picker with detailed camera info
                var options = cameras.Select(c =>
                    $"{c.Name} ({c.Position}){(c.HasFlash ? " 📸" : "")}"
                ).ToArray();

                var result = await App.Current.MainPage.DisplayActionSheet("Select Camera", "Cancel", null, options);
                if (!string.IsNullOrEmpty(result))
                {
                    var selectedIndex = options.FindIndex(result);
                    if (selectedIndex >= 0)
                    {
                        CameraControl.Facing = CameraPosition.Manual;
                        CameraControl.CameraIndex = selectedIndex;
                        CameraControl.IsOn = true;
                    }
                }
            });

        }
    }

    private void TappedTurnCamera(object sender, ControlTappedEventArgs controlTappedEventArgs)
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



    private void TappedCycleEffects(object sender, ControlTappedEventArgs controlTappedEventArgs)
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

    private async void TappedTakePicture(object sender, SkiaGesturesParameters skiaGesturesParameters)
    {
        if (CameraControl.State == CameraState.On && !CameraControl.IsBusy)
        {
            CameraControl.FlashScreen(Color.Parse("#EEFFFFFF"));
            await CameraControl.TakePicture().ConfigureAwait(false);
        }
    }

    private void TappedResume(object sender, ControlTappedEventArgs controlTappedEventArgs)
    {
        CameraControl.IsOn = true;
    }

    float step = 0.2f;

    private void Tapped_ZoomOut(object sender, SkiaGesturesParameters skiaGesturesParameters)
    {
        CameraControl.Zoom -= step;
    }

    private void Tapped_ZoomIn(object sender, SkiaGesturesParameters skiaGesturesParameters)
    {
        CameraControl.Zoom += step;
    }

    private void OnZoomed(object sender, ZoomEventArgs e)
    {
        CameraControl.Zoom = e.Value;
    }
}