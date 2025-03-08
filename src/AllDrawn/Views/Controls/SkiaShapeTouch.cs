using DrawnUi.Maui.Infrastructure;
using System.Windows.Input;

namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls;

public class SkiaShapeTouch : SkiaShape
{
    public override ISkiaGestureListener ProcessGestures(SkiaGesturesParameters args, GestureEventProcessingInfo apply)
    {
        if (args.Type == TouchActionResult.Tapped)
        {
            if (CommandTapped != null)
            {
                var pass = new SkiaTouchResultContext()
                {
                    Control = this,
                    Context = this.BindingContext,
                    TouchArgs = args.Event,
                    TouchAction = args.Type
                };
                CommandTapped.Execute(pass);

                return this;
            }
        }

        return base.ProcessGestures(args, apply);
    }

    public static readonly BindableProperty CommandTappedProperty = BindableProperty.Create(nameof(CommandTapped), typeof(ICommand),
        typeof(SkiaShapeTouch),
        null);
    public ICommand CommandTapped
    {
        get { return (ICommand)GetValue(CommandTappedProperty); }
        set { SetValue(CommandTappedProperty, value); }
    }
}