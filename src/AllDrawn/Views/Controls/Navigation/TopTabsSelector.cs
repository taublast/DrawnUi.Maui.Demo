using DrawnUi.Controls;

namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls.Navigation;

public class CustomColumnsSelector : GenericViewsSelector
{
    object lockViews = new();

    public override void UpdateIndex()
    {
        lock (lockViews)
        {
            base.UpdateIndex();

            //SelectedIndex will contain MaxColumns, from 1 to 3
            var index = SelectedIndex - 1;

            //get control of index SelectedIndex from Views
            if (index >= 0 && index < Views.Count)
            {
                var selected = Views[index];
                foreach (var skiaControl in Views)
                {
                    if (skiaControl == selected)
                    {
                        skiaControl.Scale = 1;
                        skiaControl.Opacity = 0.75;
                    }
                    else
                    {
                        skiaControl.Scale = 0.85;
                        skiaControl.Opacity = 0.75;
                    }
                }

            }
        }
    }
}

public class GenericViewsSelector : SkiaLayout
{

    public virtual void UpdateIndex()
    {

    }

    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
        nameof(SelectedIndex),
        typeof(int),
        typeof(GenericViewsSelector),
        -1, propertyChanged: ChangeIndex);

    private static void ChangeIndex(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is GenericViewsSelector control)
        {
            control.UpdateIndex();
        }
    }

    public int SelectedIndex
    {
        get { return (int)GetValue(SelectedIndexProperty); }
        set { SetValue(SelectedIndexProperty, value); }
    }


}

public class TopTabsSelector : SkiaTabsSelector
{
    public TopTabsSelector()
    {
        TabType = typeof(SkiaLabel);
    }

    public void UpdateIcons()
    {
        for (int i = 0; i < SelectableTabs.Count; i++)
        {
            if (i == SelectedIndex)
            {
                SelectableTabs[i].VIew.Opacity = 1;
            }
            else
            {
                SelectableTabs[i].VIew.Opacity = 0.66;
            }
        }
    }

    protected override void OnLayoutChanged()
    {
        base.OnLayoutChanged();

        UpdateIcons();
    }

    public override async Task OnTabSelectionChanged(bool tabsChanged, int index)
    {
        if (SelectedIndex >= 0 && !tabsChanged)
        {
            UpdateIcons();
        }

        await base.OnTabSelectionChanged(tabsChanged, index);
    }
}