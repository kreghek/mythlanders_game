using System;

namespace Client.Engine.PostProcessing;

public sealed class ShakePower
{
    public ShakePower(float value)
    {
        if (value < 0 || value > 1)
        {
            throw new ArgumentException("Power must be between 0 and 1.");
        }

        Value = value;
    }

    public float Value { get; }
}