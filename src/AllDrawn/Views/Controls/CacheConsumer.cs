using AppoMobi.Maui.DrawnUi.Enums;
using AppoMobi.Maui.DrawnUi.Views;


namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls;

public class CacheConsumer : SkiaShape
{

    public static readonly BindableProperty CacheOpacityProperty = BindableProperty.Create(
        nameof(CacheOpacity),
        typeof(double),
        typeof(CacheConsumer),
        1.0,
    propertyChanged: NeedApplyProperties);

    public double CacheOpacity
    {
        get { return (double)GetValue(CacheOpacityProperty); }
        set { SetValue(CacheOpacityProperty, value); }
    }

    public static readonly BindableProperty CacheBlurProperty = BindableProperty.Create(
        nameof(CacheBlur),
        typeof(double),
        typeof(CacheConsumer),
        0.0,
        propertyChanged: NeedApplyProperties);


    public double CacheBlur
    {
        get { return (double)GetValue(CacheBlurProperty); }
        set { SetValue(CacheBlurProperty, value); }
    }

    public static readonly BindableProperty CacheZoomProperty = BindableProperty.Create(
        nameof(CacheZoom),
        typeof(double),
        typeof(CacheConsumer),
        1.0,
        propertyChanged: NeedApplyProperties);


    public double CacheZoom
    {
        get { return (double)GetValue(CacheZoomProperty); }
        set { SetValue(CacheZoomProperty, value); }
    }

    public static readonly BindableProperty CacheSourceProperty = BindableProperty.Create(
        nameof(CacheSource),
        typeof(SkiaControl),
        typeof(CacheConsumer),
        null,
        propertyChanged: WhenSourceChanged,
        defaultBindingMode: BindingMode.OneTime);

    private SkiaImage _imageHolder;
    //    private SkiaImage _texture;

    public SkiaControl CacheSource
    {
        get { return (SkiaControl)GetValue(CacheSourceProperty); }
        set { SetValue(CacheSourceProperty, value); }
    }

    protected override void OnLayoutChanged()
    {
        base.OnLayoutChanged();

        ApplyProperties();
    }

    private static void NeedApplyProperties(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is CacheConsumer control)
        {
            control.ApplyProperties();
        }
    }


    private static void WhenSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is CacheConsumer control)
        {
            control.AttachSource();
        }
    }



    SKSurface smallerSurface = null;

    private int frameCounter = 0;

    private float lowerResolutionBy = 2f;
    private int lowerFramesBy = 1;

    //todo add feature to SkiaImage: prepare image in background and render to GPU surface to be rendered then to canvas

    private void OnSourceCacheChanged(object sender, CachedObject cache)
    {

        if (UseLowResolution)
        {
            frameCounter++;
            if (frameCounter % lowerFramesBy == 0)
            {
                // Perform the action on every x frame


                var top = (int)Math.Round((CacheSource.Height - this.Height) * this.RenderingScale);
                var left = 0;
                var right = (int)Math.Round(left + this.Width * this.RenderingScale);
                var bottom = (int)Math.Round(top + this.Height * this.RenderingScale);
                if (left >= 0 && top >= 0 && right <= cache.Image.Width && bottom <= cache.Image.Height)
                {
                    SKRectI neededArea = new SKRectI(left, top, right, bottom);

                    SKRectI scaledDownArea = new SKRectI(0, 0,
                        (int)Math.Round(neededArea.Width / lowerResolutionBy), (int)Math.Round(neededArea.Height / lowerResolutionBy));

                    // Create a small surface with the same aspect as neededArea 

                    var info = new SKImageInfo((int)scaledDownArea.Width, (int)scaledDownArea.Height);

                    if (smallerSurface == null)
                    {
                        //if (cache.Type == SkiaCacheType.GPU && Superview?.CanvasView is SkiaViewAccelerated accelerated)
                        //{
                        //    //hardware accelerated - might crash Fatal signal 11 (SIGSEGV), code 1 (SEGV_MAPERR)
                        //    smallerSurface = SKSurface.Create(accelerated.GRContext, true, info);
                        //}

                        if (smallerSurface == null)
                        {
                            //normal one
                            smallerSurface = SKSurface.Create(info);
                        }
                    }

                    var smallerCanvas = smallerSurface.Canvas;

                    SKRect sourceRect = new SKRect(neededArea.Left, neededArea.Top, neededArea.Right, neededArea.Bottom);
                    SKRect destRect = new SKRect(0, 0, scaledDownArea.Width, scaledDownArea.Height);

                    smallerCanvas.DrawImage(cache.Image, sourceRect, destRect); //<----- fps bottleneck is here

                    //var smallerImage = smallerSurface.Snapshot();
                    //_imageHolder.SetImageInternal(smallerImage);

                }
                else
                {
                    var shitHappened = true;
                }

            }
            if (frameCounter == lowerFramesBy)
            {
                frameCounter = 0;
            }

        }
        else
        {
            _imageHolder?.SetImageInternal(cache.Image);
        }

    }

    /// <summary>
    /// If this is enabled we are going to copy cache to a lower resolution surafce before using inside. Might speed up FPS and useful if your image is goinf to be blurred anyway.
    /// </summary>
    bool UseLowResolution = false;

    private void ApplyProperties()
    {
        if (_imageHolder != null)
        {
            if (UseLowResolution)
            {
                _imageHolder.VerticalOffset = 0;
                _imageHolder.UseCache = SkiaCacheType.None;
            }
            else
            {
                _imageHolder.UseCache = SkiaCacheType.None;
                _imageHolder.VerticalOffset = -(CacheSource.Height / 2f - this.Height / 2f);
            }

            _imageHolder.Opacity = this.CacheOpacity;
            _imageHolder.Blur = this.CacheBlur;
            _imageHolder.ZoomY = CacheZoom;
            _imageHolder.ZoomX = CacheZoom;
        }
    }

    public override void OnDisposing()
    {
        base.OnDisposing();

        if (CacheSource != null)
        {
            CacheSource.CreatedCache -= OnSourceCacheChanged;
            CacheSource = null;
        }

        _imageHolder = null; //will be disposed by engine as being inside Views
    }

    protected override void OnLayoutReady()
    {
        base.OnLayoutReady();

        _imageHolder = new SkiaImage()
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            Aspect = TransformAspect.AspectCover,
        };

        ApplyProperties();

        //Children.Add(_texture);
        Children.Add(_imageHolder);
    }

    /// <summary>
    /// Designed to be just one-time set
    /// </summary>
    protected void AttachSource()
    {
        if (CacheSource != null)
        {
            CacheSource.CreatedCache += OnSourceCacheChanged;
        }
    }


}