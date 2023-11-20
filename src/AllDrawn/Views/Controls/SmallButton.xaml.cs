namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls;

public partial class SmallButton : SkiaButton
{
    public SmallButton()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Clip effects with rounded rect of the frame inside
    /// </summary>
    /// <returns></returns>
    public override SKPath CreateClip(object arguments)
    {
        if (MainFrame != null)
        {
            return MainFrame.CreateClip(arguments);
        }

        return base.CreateClip(arguments);
    }

    async void AnimatePress(SkiaControl icon)
    {
        await icon.ScaleToAsync(0.9, 0.9, 50, Easing.CubicOut);
        await icon.ScaleToAsync(1.0, 1.0, 50, Easing.SpringOut);
    }

    //could also create a shadow and hide it when pressed
    public override bool OnDown(TouchActionEventArgs args, SKPoint childOffset)
    {
        this.ScaleToAsync(0.98, 0.98, 16, Easing.CubicOut);

        return base.OnDown(args, childOffset);
    }


    public override void OnUp()
    {
        this.ScaleToAsync(1.0, 1.0, 32, Easing.SpringOut);

        base.OnUp();
    }


    SkiaLabel MainLabel;
    SkiaShape MainFrame;

    protected override void OnMeasured()
    {
        base.OnMeasured();

        if (MainLabel == null)
        {
            MainLabel = this.FindView<SkiaLabel>("MainLabel");
            MainLabel.Text = Text;
        }
        if (MainFrame == null)
        {
            MainFrame = this.FindView<SkiaShape>("MainFrame");
            this.TransformView = MainFrame;
        }
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(Text))
        {
            if (MainLabel != null)
            {
                MainLabel.Text = this.Text;
            }
        }
    }
}