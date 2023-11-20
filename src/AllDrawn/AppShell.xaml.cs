using AppoMobi.Maui.DrawnUi.Controls;
using AppoMobi.Specials.Helpers;

namespace AppoMobi.Maui.DrawnUi.Demo
{
    public partial class AppShell : SkiaShell
    {
        private readonly MainPageViewModel _vm;

        public AppShell()
        {
            try
            {
                //ROUTES
                foreach (var appRoute in AppRoutes.GetRoutes())
                {
                    RegisterRoute(appRoute.Item1, appRoute.Item2);
                }

                RegisterActionRoute("lasttab", () =>
                {
                    this.NavigationLayout.SelectedIndex = 2;
                });

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
                //SkiaLabel.DebugColor = Color.Parse("#22ff0000");
#endif
                _vm = Services.GetService<MainPageViewModel>();

                BindingContext = _vm;

                InitializeComponent();

                Tasks.StartDelayedAsync(TimeSpan.FromMilliseconds(500), async () =>
                {
                    await Task.Run(() => { _vm.RefreshCommandData?.Execute(true); }).ConfigureAwait(false);
                });
            }
            catch (Exception e)
            {
                Super.DisplayException(this, e);
            }
        }



        /// <summary>
        /// Animate FADE IN after splash screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Canvas_WillFirstTimeDraw(object sender, SkiaDrawingContext e)
        {
            Canvas.Opacity = 0.001;
            await Canvas.FadeTo(1, 3000, Easing.Linear);
        }

    }
}