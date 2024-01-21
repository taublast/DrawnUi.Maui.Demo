using DrawnUi.Maui;

namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls.Navigation;

public class TabsUnderline : TabsBall
{
    public override SkiaShape CreateBall()
    {
        return new SkiaShape
        {
            Type = ShapeType.Rectangle,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
        };

    }

    public override void UpdateBall()
    {
        if (Ball != null)
        {
            var tabWidth = Width / TabsCount;

            Ball.HeightRequest = CircleRadius * 2;
            Ball.WidthRequest = tabWidth;
            Ball.BackgroundColor = Color;
            Ball.TranslationY = VerticalOffset;
        }
    }

    //CancellationTokenSource _cts;

    //protected override void Animate()
    //{
    //    _cts?.Cancel();
    //    _cts = new CancellationTokenSource();
    //    double toX = CalculateTargetHorizontalOffset(SelectedIndex);
    //    Ball.TranslateToAsync(toX, VerticalOffset, 250, Easing.SinOut, _cts);
    //}


}