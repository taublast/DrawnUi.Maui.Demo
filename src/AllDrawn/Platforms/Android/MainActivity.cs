using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AppoMobi.Maui.DrawnUi.Demo.Resources.Strings;
using AppoMobi.Specials;

namespace AppoMobi.Maui.DrawnUi.Demo
{
    [Activity(Theme = "@style/Maui.SplashTheme",
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleTask, //without this your app will start another instance upon opening push notification
        ScreenOrientation = ScreenOrientation.SensorPortrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Super.SetFullScreen(this);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            Super.SetFullScreen(this);
        }


        private bool maybe_exit;

        public override void OnBackPressed() //todo use new api
        {
            if (maybe_exit)
            {
                this.MoveTaskToBack(true);
            }

            if (!App.Shell.GoBack(true))
            {
                App.Shell.ShowToast(ResStrings.PressBACKOnceAgain);
                maybe_exit = true;

                Tasks.StartDelayedAsync(TimeSpan.FromSeconds(2), async () =>
                {
                    maybe_exit = false;
                });
            }

        }


    }
}