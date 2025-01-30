global using AppoMobi.Maui.DrawnUi.Demo.ViewModels;
global using AppoMobi.Maui.DrawnUi.Demo.Views;
global using AppoMobi.Maui.Gestures;
global using AppoMobi.Maui.Navigation;
global using AppoMobi.Specials;
global using DrawnUi.Maui.Controls;
global using DrawnUi.Maui.Draw;
global using SkiaSharp;
using Microsoft.Extensions.Logging;
using DeviceInfo = Microsoft.Maui.Devices.DeviceInfo;


namespace AppoMobi.Maui.DrawnUi.Demo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                
                .UseDrawnUi(new()
                {
                    MobileIsFullscreen = true,
                    DesktopWindow = new()
                    {
                        Width = 500,
                        Height = 800,
                        //IsFixedSize = true //user cannot resize window
                    }
                })

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "FontText");
                    fonts.AddFont("OpenSans-Semibold.ttf", "FontTextBold");
                    fonts.AddFont("SeymourOne-Regular.ttf", "FontTextTitle");
                });

            //SkiaImageManager.ReuseBitmaps = true;


#if DEBUG
            builder.Logging.AddDebug();

            //SkiaImageManager.LogEnabled = true;
#endif

            //APP INFRASTRUCTURE
            builder.Services.AddSingleton<NavigationViewModel>();

            //UI
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<ScrollingCellsViewModel>();
            builder.Services.AddTransient<SomeTabsViewModel>();
            builder.Services.AddTransient<SimplePageViewModel>();
            builder.Services.AddTransient<TakePictureViewModel>();
            builder.Services.AddTransient<ItemDetailsViewModel>();
            builder.Services.AddTransient<ScreenItemDetails>();
            builder.Services.AddTransient<ScreenCarousel>();
            builder.Services.AddTransient<ScreenCameraPhoto>();
            builder.Services.AddTransient<ScreenControls>();

            builder.Services.AddTransient<ProjectViewModel>();
            builder.Services.AddTransient<TestPage>();

            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }

        #region STATIC

        public static bool IsWindows
        {
            get
            {
                return DeviceInfo.Platform == DevicePlatform.WinUI;
            }
        }

        /// <summary>
        /// SInce i don't like iPhone getting hot while using Metal for this project it's not really needed.
        /// On windows it's not supported at all so..
        /// </summary>
        public static bool UseHardwareAcceleration
        {
            get
            {

#if ANDROID
                return true;
#else
                return false;
#endif
            }
        }

        public static double NavBarHeight => Super.NavBarHeight;
        public static double BottomTabHeight => Super.BottomTabsHeight;

        public static string Name => AppInfo.Current.Name;
        public static string Build => AppInfo.Current.BuildString;
        public static string BundleId => AppInfo.Current.PackageName;
        public static string Version => AppInfo.Current.VersionString;

        #endregion
    }
}