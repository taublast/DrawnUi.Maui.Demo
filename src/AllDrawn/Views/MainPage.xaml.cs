using AppoMobi.Specials.Helpers;

namespace AppoMobi.Maui.DrawnUi.Demo
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Tasks.StartDelayed(TimeSpan.FromSeconds(2), TouchEffect.CloseKeyboard);
        }

        public MainPage()
        {
            try
            {
                var vm = App.Instance.Services.GetService<MainPageViewModel>();

                BindingContext = vm;

                InitializeComponent();

                vm.Presentation.Shell.Initialize(this.Canvas);

                Tasks.StartDelayedAsync(TimeSpan.FromMilliseconds(500), async () =>
                {
                    await Task.Run(() => { vm.RefreshCommandData?.Execute(true); }).ConfigureAwait(false);
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
            await Canvas.FadeTo(1, 2000, Easing.Linear);
        }

    }
}