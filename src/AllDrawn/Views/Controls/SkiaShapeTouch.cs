using System.Windows.Input;
using AppoMobi.Maui.DrawnUi.Drawn.Infrastructure.Interfaces;
using AppoMobi.Maui.DrawnUi.Infrastructure;

namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls;

public class SkiaShapeTouch : SkiaShape
{
    public override ISkiaGestureListener OnSkiaGestureEvent(TouchActionType type, TouchActionEventArgs args, TouchActionResult touchAction,
        SKPoint childOffset, SKPoint childOffsetDirect, ISkiaGestureListener wasConsumed)
    {
        if (touchAction == TouchActionResult.Tapped)
        {
            if (CommandTapped != null)
            {
                var pass = new SkiaTouchResultContext()
                {
                    Control = this,
                    Context = this.BindingContext,
                    TouchArgs = args,
                    TouchAction = touchAction
                };
                CommandTapped.Execute(pass);
                return this;
            }
        }

        return base.OnSkiaGestureEvent(type, args, touchAction, childOffset, childOffsetDirect, wasConsumed);
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