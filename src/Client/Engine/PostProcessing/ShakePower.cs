using System;

namespace Client.Engine.PostProcessing;

public sealed class ShakePower
{
    /// <summary>
    /// Create shake effect power between 0 to 1. Its recommended to use in-box powers from <see cref="ShakePowers" />
    /// </summary>
    /// <param name="value">Numeric value of power.</param>
    /// <exception cref="ArgumentException"></exception>
    public ShakePower(float value)
    {
        if (value is < 0 or > 1)
        {
            throw new ArgumentException("Power must be between 0 and 1.");
        }

        Value = value;
    }

    public float Value { get; }
}

public static class ShakePowers
{
    public static ShakePower Normal { get; } = new ShakePower(0.02f);
}