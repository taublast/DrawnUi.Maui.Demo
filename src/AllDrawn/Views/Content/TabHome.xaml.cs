using AppoMobi.Maui.DrawnUi.Drawn.Animate;
using System.Diagnostics;

namespace AppoMobi.Maui.DrawnUi.Demo.Views.Content;

public partial class TabScrollCells
{
    public TabScrollCells()
    {
        InitializeComponent();
    }

    private void OnEventStackCellsRendered(object sender, EventArgs e)
    {
        if (sender is SkiaLayout layout)
        {
            var lastCell = layout.RenderTree.LastOrDefault();
            if (lastCell != null)
            {
                var lastVisibleIndex = lastCell.Index;
                //todo  preload images of some still hidden items like +3 indexes away..

            }
        }
    }

}

public class AnimateVerticalStack : AnimateHorizontalStack
{


    protected override void SetupForAnimation()
    {
        this.Opacity = 0;
        this.TranslationY = Parent.Height;
        readyForAnimation = true;
    }

    protected override async Task Animate()
    {
        await this.AnimateWith(
            (c) => c.FadeToAsync(1, 700),
            (c) => c.TranslateToAsync(0, 0, 500));
    }

    public override ScaledSize Measure(float widthConstraint, float heightConstraint, float scale)
    {
        var check = this._emptyView.IsVisible;

        return base.Measure(widthConstraint, heightConstraint, scale);
    }

    protected override void OnMeasured()
    {
        base.OnMeasured();

    }

    protected override bool ApplyIsEmpty(bool value)
    {
        if (!value && wasEmpty)
        {
            //overriding to fade out
            _emptyView?.FadeToAsync(0, 300).ContinueWith(async (s) =>
            {
                await Task.Delay(10); //update ui with last opacity=0
                base.ApplyIsEmpty(value);
            });

            return true; //fake
        }

        return base.ApplyIsEmpty(value);
    }
}

public class AnimateHorizontalStack : SkiaLayout
{
    uint speed = 750;

    bool animated = false;
    bool hasData;
    protected bool readyForAnimation;

    public override ScaledSize Measure(float widthConstraint, float heightConstraint, float scale)
    {
        return base.Measure(widthConstraint, heightConstraint, scale);
    }

    public AnimateHorizontalStack()
    {
        this.IsEmptyChanged += OnEmptyChanged;
    }

    protected bool wasEmpty;

    private async void OnEmptyChanged(object sender, bool e)
    {
        wasEmpty = e;

        if (!e)
        {
            SetupForAnimation();
            await Task.Delay(10);
            await Animate();
        }
    }

    protected override async void OnLayoutChanged()
    {
        base.OnLayoutChanged();

        await AnimateOnAppearing();
    }

    protected virtual void SetupForAnimation()
    {
        this.Opacity = 0;
        this.TranslationX = Parent.Width;
        readyForAnimation = true;
    }

    protected virtual async Task Animate()
    {
        await this.AnimateWith(
            (c) => c.FadeToAsync(1, speed),
            (c) => c.TranslateToAsync(0, 0, speed));
    }

    async Task<bool> AnimateOnAppearing()
    {
        //animate onappearing
        if (ItemsSource != null && LastParentVisiblity && readyForAnimation)
        {
            if (ItemsSource.Count > 0)
            {
                //looks like we loaded something?
                if (!hasData) //animate once
                {
                    SetupForAnimation();
                    readyForAnimation = false;
                    hasData = true;
                    await Animate();
                    animated = true;
                    return true;
                }
            }
            else
            {
                hasData = false;
            }
        }
        return false;
    }

    public override void OnVisibilityChanged(bool newvalue)
    {
        base.OnVisibilityChanged(newvalue);

        if (newvalue && LayoutReady)
        {
            AnimateOnAppearing();
        }
    }

}

public class EmptyTest : SkiaLayout
{
    public override void Render(SkiaDrawingContext context, SKRect destination, float scale)
    {
        Debug.WriteLine($"[EMPTY] {destination}");

        base.Render(context, destination, scale);
    }

    public override ScaledSize Measure(float widthConstraint, float heightConstraint, float scale)
    {
        return base.Measure(widthConstraint, heightConstraint, scale);
    }
}