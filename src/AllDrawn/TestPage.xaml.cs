namespace AppoMobi.Maui.DrawnUi.Demo;

public class ControlBlinking : SkiaControl
{
    private BlinkAnimator _blinkAnimator;


    protected override void OnLayoutReady()
    {
        base.OnLayoutReady();

        if (_blinkAnimator == null)
        {
            _blinkAnimator = new BlinkAnimator(this)
            {
                Repeat = -1,
                Speed = 280,
                ColorsRatio = 0.66,
                Color1 = Color.FromArgb("#36A7CB"),
                Color2 = Color.FromArgb("#CB0000"),
                OnUpdated = (value) =>
                {
                    BackgroundColor = _blinkAnimator.CurrentColor;
                }
            };
        }

        if (!_blinkAnimator.IsRunning)
        {
            _blinkAnimator.Start();
        }

    }

}

public partial class TestPage
{

    public TestPage(ProjectViewModel vm)
    {
        try
        {
            SkiaShell.PopupBackgroundColor = Color.Parse("#33000000");

            BindingContext = _vm = vm;

            InitializeComponent();
        }
        catch (Exception e)
        {
            Super.DisplayException(this, e);
        }
    }

    public TestPage()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception e)
        {
            Super.DisplayException(this, e);
        }
    }

    bool once;

    private readonly ProjectViewModel _vm;

    public override async void OnAppearing()
    {
        base.OnAppearing();

        if (!once)
        {
            once = true;


        }
    }



}