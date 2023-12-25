using System;

namespace Client.Assets;

/// <summary>
/// Content attribute to assign theme decoration to location.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
internal sealed class LocationThemeAttribute : Attribute
{
    public LocationThemeAttribute(LocationTheme theme)
    {
        Theme = theme;
    }

    public LocationTheme Theme { get; }
}