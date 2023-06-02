using System.Diagnostics;
using System.Reflection;

using Client.Assets;
using Client.Core;

namespace Client.GameScreens;

/// <summary>
/// Utility to map location and location theme to draw game screen backgrounds and environment.
/// </summary>
internal static class LocationThemeHelper
{
    public static LocationTheme GetBackgroundType(ILocationSid regularTheme)
    {
        var themeAttr = regularTheme.GetType().GetCustomAttribute<LocationThemeAttribute>();
        if (themeAttr is null)
        {
            Debug.Fail("Location theme is not defined.");
        }

        return themeAttr.Theme;
    }
}