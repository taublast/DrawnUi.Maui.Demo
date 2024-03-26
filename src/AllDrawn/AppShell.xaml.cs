using AppoMobi.Maui.DrawnUi.Demo.ViewModels;
using AppoMobi.Maui.DrawnUi.Demo.Views;
using DrawnUi.Maui.Controls;

namespace AppoMobi.Maui.DrawnUi.Demo;

public partial class AppShell : SkiaShell
{
    private readonly MainPageViewModel _vm;

    /// <summary>
    /// Not to be used, so made protected
    /// </summary>
    protected new INavigation Navigation { get; set; }

    public AppShell()
    {
        try
        {
            Super.SetLocale("en");

            //ROUTES
            foreach (var appRoute in AppRoutes.GetRoutes())
            {
                RegisterRoute(appRoute.Item1, appRoute.Item2);
            }

            RegisterActionRoute("lasttab", () =>
            {
                this.NavigationLayout.SelectedIndex = 2;
            });

            RegisterRoute("root", typeof(MainPage));

            //UI tweaks
            Super.BottomTabsHeight = 64;
            Super.NavBarHeight = 47;
            ToastTextFont = "FontText";
            ToastTextFontWeight = 600;
            ToastTextColor = Colors.GreenYellow;

#if DEBUG
            //ViewsAdapter.LogEnabled = true;
            //SkiaImageManager.LogEnabled = true;
            //TouchEffect.LogEnabled = true;
            //SkiaShell.LogEnabled = true;
            //SkiaLabel.DebugColor = Color.Parse("#22ff0000");
            //DrawnUi.Views.Canvas.DebugGesturesColor=Color.Parse("#00ff0000");
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

    public override void OnNavBarInvalidated()
    {
        base.OnNavBarInvalidated();

        _vm.Presentation.UpdateControls();
    }


    /// <summary>
    /// Animate FADE IN after splash screen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Canvas_WillFirstTimeDraw(object sender, SkiaDrawingContext? ctx)
    {
        Canvas.Opacity = 0.001;
        await Canvas.FadeTo(1, 2500, Easing.Linear);
    }

}