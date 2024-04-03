using System.Diagnostics;

namespace AppoMobi.Maui.DrawnUi.Demo.Views;

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


}

public class AnimateHorizontalStack : SkiaLayout
{

    bool animated = false;
    bool hasData;
    protected bool readyForAnimation;


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
            (c) => c.FadeToAsync(1, 700),
            (c) => c.TranslateToAsync(0, 0, 350));
    }

    async Task<bool> AnimateOnAppearing()
    {
        //animate onappearing
        if (ItemsSource != null && IsVisibleInViewTree() && readyForAnimation)
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
            AnimateOnAppearing().ConfigureAwait(false);
        }
    }

}
