using AppoMobi.Maui.DrawnUi.Demo.Views.Content;
using AppoMobi.Maui.DrawnUi.Navigation;

namespace AppoMobi.Maui.DrawnUi.Demo
{
    public class AppShell : SkiaShell
    {
        private readonly NavigationViewModel _vm;

        public AppShell(NavigationViewModel vm, IServiceProvider services) : base(services)
        {
            _vm = vm;

            RegisterRoute("tabs", typeof(MainPage));
            RegisterRoute("controls", typeof(ScreenControls));
            RegisterRoute("details", typeof(ScreenItemDetails));
            RegisterRoute("carousel", typeof(ScreenCarousel));

            //UI tweaks
            Super.BottomTabsHeight = 56;
            Super.NavBarHeight = 47;
            ToastTextFont = "FontText";
            ToastTextFontWeight = 600;
            ToastTextColor = Colors.GreenYellow;


#if DEBUG
            //ViewsAdapter.LogEnabled = true;
            //SkiaImageLoadingManager.LogEnabled = true;
            //TouchEffect.LogEnabled = true;
            //SkiaShell.LogEnabled = true;
#endif
        }


        protected override void OnStarted()
        {
            base.OnStarted();

            App.Current.MainPage = this;
        }

        public override void OnNavBarInvalidated()
        {
            base.OnNavBarInvalidated();

            _vm.UpdateControls();
        }


    }
}
