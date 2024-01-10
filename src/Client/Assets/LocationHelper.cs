using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Client.Core;

namespace Client.Assets;

/// <summary>
/// Utility to work with location metadata.
/// </summary>
internal static class LocationHelper
{
    public static IReadOnlyCollection<ILocationSid> GetAllLocation()
    {
        return GetAllFromStaticCatalog<ILocationSid>(typeof(LocationSids));
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

    public static LocationTheme GetLocationTheme(ILocationSid location)
    {
        var themeAttr = GetLocationMetadataAttribute<LocationThemeAttribute>(location);
        if (themeAttr is null)
        {
            Debug.Fail("Location theme is not defined.");
        }

        return themeAttr.Theme;
    }

    public static ILocationSid? ParseLocationFromCatalog(string storedLocationSid)
    {
        var locations = GetAllLocation().Cast<LocationSid>();

        return locations.SingleOrDefault(x =>
            x.Key.Equals(storedLocationSid, StringComparison.InvariantCultureIgnoreCase));
    }

    private static IReadOnlyCollection<TObj> GetAllFromStaticCatalog<TObj>(Type catalog)
    {
        return catalog
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.PropertyType == typeof(TObj))
            .Select(f => f.GetValue(null))
            .Where(v => v is not null)
            .Select(v => (TObj)v!)
            .ToArray();
    }

    /// <summary>
    /// Get metadata attribute from location sids catalog.
    /// </summary>
    /// <returns>Returns attribute of null. Null applicable only for debug/development.</returns>
    private static TMetadataAttribute? GetLocationMetadataAttribute<TMetadataAttribute>(ILocationSid location)
        where TMetadataAttribute : Attribute
    {
        var sids = typeof(LocationSids).GetProperties(BindingFlags.Static | BindingFlags.Public);
        var locationProp = sids.Single(x => x.GetValue(null) == location);

        var metadataAttribute = locationProp.GetCustomAttribute<TMetadataAttribute>();
        return metadataAttribute;
    }
}