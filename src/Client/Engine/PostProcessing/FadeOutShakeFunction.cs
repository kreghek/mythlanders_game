using System;

namespace Client.Engine.PostProcessing;

public sealed class FadeOutShakeFunction : IShakeFunction
{
    private readonly ShakePower _basePower;

    public FadeOutShakeFunction(ShakePower basePower)
    {
        _basePower = basePower;
    }

    public ShakePower Calculate(double t)
    {
        var v = _basePower.Value * Math.Clamp(t, 0, 1);
        return new ShakePower((float)v);
    }
}