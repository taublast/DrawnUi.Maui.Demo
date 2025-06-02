namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls;

public class CellWIthHorizontalParallax : FastCellWithBanner
{
    public override void OnScrolled()
    {
        base.OnScrolled();

        UpdateParallax();
    }

    void UpdateParallax()
    {
        // for cell-aware parallax
        // the idea is to offset the image from the center of the screen
        // (so the parallax value is 0 when cell is in the middle of the screen)
        double horizontalOffset = CalculateHorizontalOffsetPts();
        double parallaxValue = CalculateParallaxValue(horizontalOffset);

        // Apply the parallax effect to the images inside the cell
        if (ImageBanner != null)
        {
            ImageBanner.TranslationX = parallaxValue;
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        UpdateParallax();
    }

    /// <summary>
    /// Calculate the horizontal offset of a cell from the center of the screen
    /// </summary>
    /// <returns></returns>
    double CalculateHorizontalOffsetPts()
    {
        // Calculate the cell's horizontal position relative to the center of the screen
        double ptsCenterX = this.GetPositionOnCanvasInPoints().X + this.Width / 2.0; // Get the cell's center X position

        var screenCenterX = App.Shell.RootLayout.MeasuredSize.Units.Width / 2f; //center of the screen

        return ptsCenterX - screenCenterX;
    }

    /// <summary>
    /// Calculate the parallax value based on the horizontal offset
    /// </summary>
    /// <param name="horizontalOffset"></param>
    /// <returns></returns>
    double CalculateParallaxValue(double horizontalOffset)
    {
        double scalingFactor = 0.1;
        return -horizontalOffset * scalingFactor;
    }
}