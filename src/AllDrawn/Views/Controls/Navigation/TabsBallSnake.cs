using DrawnUi.Maui;

namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls.Navigation
{

    /// <summary>
    /// A ball that moves between tabs simulating a snake
    /// </summary>
    public class TabsBallSnake : TabsBall
    {
        CancellationTokenSource _cts;

        protected override async void Animate()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            var previousIndexX = CalculateTargetHorizontalOffset(LastSelectedIndex);
            var newIndexX = CalculateTargetHorizontalOffset(SelectedIndex);

            var pathX = newIndexX - previousIndexX;
            var minSnakeWidth = CircleRadius * 2;
            var maxSnakeWidth = Math.Abs(pathX) + minSnakeWidth;
            var movingLeft = newIndexX < previousIndexX;

            //restore position if started in the middle of previous animation
            //Ball.TranslationX = previousIndexX;
            //Ball.WidthRequest = minSnakeWidth;

            var speed = AnimationSpeedMs;

            //grow to target 
            await AnimateAsync((range) =>
            {
                //range is 0-1 and eased
                double width = minSnakeWidth + (maxSnakeWidth - minSnakeWidth) * range;
                if (movingLeft)
                {
                    Ball.TranslationX = (float)(previousIndexX - (width - minSnakeWidth));
                }
                else
                {
                    Ball.TranslationX = (float)(previousIndexX);
                }
                Ball.WidthRequest = width;

            },
                () =>
                {
                    double width = minSnakeWidth + (maxSnakeWidth - minSnakeWidth);
                    if (movingLeft)
                    {
                        Ball.TranslationX = (float)(previousIndexX - (width - minSnakeWidth));
                    }
                    else
                    {
                        Ball.TranslationX = (float)(previousIndexX);
                    }
                    Ball.WidthRequest = width;
                },
                speed, Easing.CubicOut, _cts);

            //collapse to target 
            await AnimateAsync((range) =>
            {
                //range is 0-1 and eased
                var width = maxSnakeWidth - (maxSnakeWidth - minSnakeWidth) * range;
                if (movingLeft)
                {
                    Ball.TranslationX = newIndexX;
                }
                else
                {
                    Ball.TranslationX = (float)(previousIndexX + pathX * range);
                }
                Ball.WidthRequest = width;

            },
                () =>
                {
                    var width = maxSnakeWidth - (maxSnakeWidth - minSnakeWidth);
                    if (movingLeft)
                    {
                        Ball.TranslationX = newIndexX;
                    }
                    else
                    {
                        Ball.TranslationX = (float)(previousIndexX + pathX);
                    }
                    Ball.WidthRequest = width;
                },

                speed, Easing.CubicIn, _cts);
        }


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
                Ball.HeightRequest = CircleRadius * 2;
                Ball.WidthRequest = CircleRadius * 2;
                Ball.CornerRadius = new Thickness(CircleRadius); //for each of 4 corners
                Ball.BackgroundColor = Color;
                Ball.TranslationY = VerticalOffset;

            }
        }

    }
}
