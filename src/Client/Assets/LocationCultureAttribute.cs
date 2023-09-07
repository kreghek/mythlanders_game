using System;

namespace Client.Assets;

/// <summary>
/// Content attribute to assign culture to location.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
internal sealed class LocationCultureAttribute : Attribute
{
    public LocationCultureAttribute(LocationCulture culture)
    {
        Culture = culture;
    }

    public LocationCulture Culture { get; }
}