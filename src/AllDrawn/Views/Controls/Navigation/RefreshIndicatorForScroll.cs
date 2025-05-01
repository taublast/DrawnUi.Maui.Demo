using System.Diagnostics;

namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls.Navigation;

public class ScrollRefreshIndicator : RefreshIndicator
{
    protected SkiaLottie Loader;

    public override void SetDragRatio(float ratio, float ptsScrollOffset, double ptsLimit)
    {
        base.SetDragRatio(ratio, ptsScrollOffset, ptsLimit);

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
                Debug.WriteLine($"[Loader] STOP");
                Loader.Stop();
            }
            else
            {
                Debug.WriteLine($"[Loader] PLAY");
                Loader.Start();
            }
        }
    }

    public override void OnParentVisibilityChanged(bool newvalue)
    {
        base.OnParentVisibilityChanged(newvalue);

        Loader?.Stop();
    }

    public override void OnVisibilityChanged(bool newvalue)
    {
        base.OnVisibilityChanged(newvalue);

        Loader?.Stop();
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