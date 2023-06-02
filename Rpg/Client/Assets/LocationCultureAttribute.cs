using System;

namespace Client.Assets;

/// <summary>
/// Content attribute to assign culture to location.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
sealed class LocationCultureAttribute : Attribute
{
    public LocationCultureAttribute(LocationCulture culture)
    {
        Culture = culture;
    }

    public LocationCulture Culture { get; }
}
