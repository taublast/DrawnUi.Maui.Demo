namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls.Navigation;

public class ScrollRefreshIndicator : RefreshIndicator
{
    protected SkiaLottie Loader;
    public ScrollRefreshIndicator()
    {


    }

    public override void SetDragRatio(float ratio)
    {
        base.SetDragRatio(ratio);

        if (FindLoader() && !IsRunning)
        {
            Loader.Seek(Loader.GetFrameAt(ratio));
        }
    }

    protected override void OnIsRunningChanged(bool value)
    {
        base.OnIsRunningChanged(value);

        if (FindLoader())
        {
            if (!value)
            {
                if (Loader.IsPlaying)
                    Loader.Stop();
            }
            else
            {
                if (!Loader.IsPlaying)
                    Loader.Start();
            }
        }
    }

    bool FindLoader()
    {
        if (Loader == null)
        {
            Loader = this.FindView<SkiaLottie>("Loader");
        }
        return Loader != null;
    }
}