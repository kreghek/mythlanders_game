using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Client.Assets;
using Client.Core;

namespace Client.GameScreens;

/// <summary>
/// Utility to work with location metadata.
/// </summary>
internal static class LocationHelper
{
    /// <summary>
    /// Get metadata attribute from location sids catalog.
    /// </summary>
    /// <returns>Returns attribute of null. Null applicable only for debug/development.</returns>
    private static TMetadataAttribute? GetLocationMetadataAttribute<TMetadataAttribute>(ILocationSid location) where TMetadataAttribute:Attribute
    {
        var sids = typeof(LocationSids).GetProperties(BindingFlags.Static | BindingFlags.Public);
        var locationProp = sids.Single(x => x.GetValue(null) == location);

        var metadataAttribute = locationProp.GetCustomAttribute<TMetadataAttribute>();
        return metadataAttribute;
    }

    public static LocationTheme GetLocationTheme(ILocationSid location)
    {
        var themeAttr = GetLocationMetadataAttribute<LocationThemeAttribute>(location);
        if (themeAttr is null)
        {
            Debug.Fail("Location theme is not defined.");
        }

        return themeAttr.Theme;
    }

    public static LocationCulture GetLocationCulture(ILocationSid location)
    {
        var cultureAttr = GetLocationMetadataAttribute<LocationCultureAttribute>(location);
        if (cultureAttr is null)
        {
            Debug.Fail("Location culture is not defined.");
        }

        return cultureAttr.Culture;
    }
}