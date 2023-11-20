using AppoMobi.Maui.DrawnUi.Demo.Views.Content;
using System.Reflection;

namespace AppoMobi.Maui.DrawnUi.Demo;

/// <summary>
/// For intellisense to help selecting routes for screens. Ex: await App.Shell.GoToAsync(AppScreens.Controls.Route, true);
/// </summary>
public static class AppRoutes
{
    public static readonly (string Route, Type Type) Root = ("root", typeof(MainScreen));
    public static readonly (string Route, Type Type) Details = ("details", typeof(ScreenItemDetails));
    public static readonly (string Route, Type Type) Controls = ("controls", typeof(ScreenControls));
    public static readonly (string Route, Type Type) Carousel = ("carousel", typeof(ScreenCarousel));

    public static IEnumerable<(string, Type)> GetRoutes()
    {
        return typeof(AppRoutes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            //.Where(f => f.FieldType == typeof((string, Type)))
            .Select(f => ((string, Type))f.GetValue(null));
    }
}