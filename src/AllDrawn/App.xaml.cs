using AppoMobi.Maui.DrawnUi.Demo.Resources.Strings;
using AppoMobi.Maui.DrawnUi.Demo.ViewModels;
using DrawnUi.Maui;
using DrawnUi.Maui.Controls;
using System.Globalization;

namespace AppoMobi.Maui.DrawnUi.Demo
{
    public partial class App : Application
    {
        public App(IServiceProvider services)
        {
            Services = services;

            InitializeComponent();

            Boostrap();
        }

        void ResizeWindow(Window window)
        {
            Super.Screen.Density = Microsoft.Maui.Devices.DeviceDisplay.Current.MainDisplayInfo.Density;

            const int DefaultWidth = 500;
            const int DefaultHeight = 800;

            // change window size.
            window.Width = DefaultWidth;
            window.Height = DefaultHeight;

            var disp = DeviceDisplay.Current.MainDisplayInfo;

            // move to screen center
            window.X = (disp.Width / disp.Density - window.Width) / 2;
            window.Y = (disp.Height / disp.Density - window.Height) / 2;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            #region UI TWEAKS

            Super.ColorAccent = Color.FromArgb("FF6A47");
            Super.ColorPrimary = Color.FromArgb("343C45");

            var nav = App.Instance.MainPage as NavigationPage;
            if (App.Instance.RequestedTheme == AppTheme.Dark)
            {
#if ANDROID

                Super.SetNavigationBarColor(Color.Parse("#11161D"), Color.Parse("#11161D"), true);

#endif
                if (nav != null)
                {
                    nav.BarBackgroundColor = Colors.Black;
                    nav.BarTextColor = Colors.White;
                }
            }
            else
            {

#if ANDROID

                Super.SetNavigationBarColor(Color.Parse("#11161D"), Color.Parse("#11161D"), true);

                //Super.SetNavigationBarColor(Colors.White, Colors.Transparent, false);
                //Super.Native?.SetNavigationBarColor(AppResources.Get<Color>("ColorPrimary"), false);
#endif
                if (nav != null)
                {
                    nav.BarBackgroundColor = Colors.Black;
                    nav.BarTextColor = Colors.White;
                    //nav.BarBackgroundColor = Colors.White;
                    //nav.BarTextColor = Colors.Black;
                }
            }

            #endregion

            return window;
        }


        public IServiceProvider Services { get; }

        void Boostrap()
        {
            SetupCulture(new EnabledLanguage[]
            {
                new EnabledLanguage
                {
                    Code = "en", Display = "English"
                },
                new EnabledLanguage
                {
                    Code = "es", Display = "Español"
                },
                new EnabledLanguage
                {
                    Code = "fr", Display = "Français"
                },
                new EnabledLanguage
                {
                    Code = "de", Display = "Deutsch"
                },
                new EnabledLanguage
                {
                    Code = "ru", Display = "Русский"
                },
            });

            Presentation.Initialize(AppRoutes.Root.Route);
        }

        public static App Instance => App.Current as App;

        public static SkiaShell Shell => Instance.Presentation.Shell;

        private NavigationViewModel _navigationVm;
        public NavigationViewModel Presentation
        {
            get
            {
                if (_navigationVm == null)
                    _navigationVm = this.Services.GetService<NavigationViewModel>();
                return _navigationVm;
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            Tasks.StartDelayed(TimeSpan.FromSeconds(3), () =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DeviceDisplay.Current.KeepScreenOn = true;
                });
            });
        }

        protected override void OnSleep()
        {
            base.OnSleep();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                DeviceDisplay.Current.KeepScreenOn = false;
            });
        }

        protected override void OnResume()
        {
            base.OnResume();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                DeviceDisplay.Current.KeepScreenOn = true;
            });

        }


        #region LOCALIZATION

        public class EnabledLanguage
        {
            public string Code { get; set; }
            public string Display { get; set; }
        }

        public static IEnumerable<EnabledLanguage> EnabledLanguages;

        public void SetLocale(string lang)
        {
            ResStrings.Culture = CultureInfo.CreateSpecificCulture(lang);
            Thread.CurrentThread.CurrentCulture = ResStrings.Culture;
            Thread.CurrentThread.CurrentUICulture = ResStrings.Culture;
            SelectedLang = lang;
            //Settings.Set("Lang", lang);
        }

        public void SetupCulture(IEnumerable<EnabledLanguage> allowed)
        {
            EnabledLanguages = allowed;

            var customLang = string.Empty;// Settings.Get("Lang", string.Empty);
            if (string.IsNullOrEmpty(customLang))
            {
                //looks like forst run, so set current device culture
                var current = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower();
                if (allowed.All(a => a.Code != current))
                {
                    current = allowed.First().Code;
                }
                customLang = current;
            }
            SetLocale(customLang);
        }

        public static string SelectedLang { get; protected set; }

        public void ChangeLanguage(string language)
        {
            SetLocale(language);
            Boostrap();
        }



        #endregion


    }
}