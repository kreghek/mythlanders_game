using System;

namespace Client.Assets;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
sealed class BiomeCultureAttribute : Attribute
{
    readonly LocationCulture _biomeCulture;

    // This is a positional argument
    public BiomeCultureAttribute(LocationCulture biomeCulture)
    {
        _biomeCulture = biomeCulture;
    }

    public LocationCulture Culture => _biomeCulture;
}