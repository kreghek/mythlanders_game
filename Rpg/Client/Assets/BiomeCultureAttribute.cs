using System;

namespace Client.Assets;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
sealed class BiomeCultureAttribute : Attribute
{
    readonly BiomeCulture _biomeCulture;

    // This is a positional argument
    public BiomeCultureAttribute(BiomeCulture biomeCulture)
    {
        _biomeCulture = biomeCulture;
    }

    public BiomeCulture Culture => _biomeCulture;
}