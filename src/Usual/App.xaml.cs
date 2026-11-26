namespace SomeMauiApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

        }

        void ResizeWindow(Window window)
        {
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
            var window = new Window(new AppShell());

#if WINDOWS

			ResizeWindow(window);

#elif MACCATALYST

            ResizeWindow(window);

#endif


            return window;
        }
    }
}