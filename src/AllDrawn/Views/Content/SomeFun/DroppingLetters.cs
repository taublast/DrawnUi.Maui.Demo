using System.Diagnostics;

namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls;

public class DroppingLetters : SkiaLabel
{
    public DroppingLetters()
    {
    }

    protected override SKRect GetCacheRecordingArea(SKRect drawingRect)
    {
        return Superview.Destination; //record whole area because we are animating letters outside of the DrawingRect
    }

    public override void OnDisposing()
    {
        StopAnimators();
        foreach (var skiaAnimator in _animators)
        {
            skiaAnimator?.Dispose();
        }
        _animators.Clear();

        base.OnDisposing();
    }

    protected override void OnPropertyChanged(string propertyName = "")
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(Text))
        {
            StopAnimators();
        }
    }
    protected override void DrawCharacter(SKCanvas canvas, int lineIndex,
        int letterIndex,
        ReadOnlySpan<char> characters,
        float x, float y,
        SKPaint paint,
        SKPaint paintStroke, SKPaint paintShadow,
        SKRect destination, float scale)
    {
        // 1 line enabled only!
        if (lineIndex == 0 && _letterOffsetsY != null)
        {
            var animated = _letterOffsetsY[letterIndex] != double.NegativeInfinity;
            if (animated)
            {

                var offsetY = (float)(_letterOffsetsY[letterIndex] * scale);

                //move gradient
                var dest = new SKRect(destination.Left, destination.Top - offsetY, destination.Right,
                    destination.Bottom - offsetY);
                SetupGradient(paint, FillGradient, dest);

                if (paintStroke != null)
                {
                    SetupGradient(paintStroke, StrokeGradient, dest);
                }

                base.DrawCharacter(canvas, lineIndex, letterIndex, characters,
                    x, y - offsetY, paint, paintStroke, paintShadow, destination, scale);
            }
        }
    }

    protected override void OnTextChanged(string value)
    {
        base.OnTextChanged(value);

        BuildAnimators();
    }

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly object _cancellationLock = new object();

    private void CancelAnimation()
    {
        lock (_cancellationLock)
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }

    public async Task StartAnimationAsync()
    {
        CancelAnimation();
        var token = _cancellationTokenSource.Token;

        try
        {

            SetupAnimators();

            if (_letterOffsetsY != null && _animators.Count > 0)
            {
                foreach (var animator in _animators)
                {
                    await Task.Delay(50, token).ConfigureAwait(false);
                    animator.Start();
                }
            }

        }
        catch (OperationCanceledException)
        {
            //Debug.WriteLine("Animation was canceled.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Animation error: {ex.Message}");
        }
    }

    public void StartAnimation()
    {
        _ = StartAnimationAsync();
    }

    public void BuildAnimators()
    {
        StopAnimators();
        foreach (var skiaAnimator in _animators)
        {
            skiaAnimator?.Dispose();
        }
        _animators.Clear();

        _letterOffsetsY = new double[this.Text.Length];
        Array.Fill(_letterOffsetsY, double.NegativeInfinity);
        if (!string.IsNullOrEmpty(this.Text))
        {

            for (int i = 0; i < Text.Length; i++)
            {
                var index = i;
                var animator = new PendulumAnimator(this, (value) =>
                {
                    try
                    {
                        _letterOffsetsY[index] = value;// * MeasuredSize.Scale;
                        Update();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                })
                {
                    IsOneDirectional = true,
                    Speed = 3.0,
                    mMinValue = 0,
                    //mMaxValue = max,
                    //Amplitude = max,
                    Gravity = 9.8,
                    AirResistance = 1.0
                };
                _animators.Add(animator);

            }
        }
    }

    public void SetupAnimators()
    {
        StopAnimators();
        _letterOffsetsY = new double[this.Text.Length];
        Array.Fill(_letterOffsetsY, double.NegativeInfinity);
        if (!string.IsNullOrEmpty(this.Text))
        {
            var max = GetParentElement(this).Height;
            for (int i = 0; i < Text.Length; i++)
            {
                var index = i;
                _animators[index].mMaxValue = max;
                _animators[index].Amplitude = max;
                _letterOffsetsY[index] = -max;
            }
        }
    }

    public void StopAnimators()
    {
        foreach (var animator in _animators.ToList())
        {
            animator.Stop();
        }
    }

    private double[] _letterOffsetsY;

    private List<PendulumAnimator> _animators = new();

}