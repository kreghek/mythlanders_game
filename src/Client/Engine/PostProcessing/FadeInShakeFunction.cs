using System;

namespace Client.Engine.PostProcessing;

//TODO Use it in archer arrow rain
public sealed class FadeInShakeFunction : IShakeFunction
{
    private readonly ShakePower _basePower;

    public FadeInShakeFunction(ShakePower basePower)
    {
        _basePower = basePower;
    }

    public ShakePower Calculate(double t)
    {
        var v = _basePower.Value * Math.Clamp(1 - t, 0, 1);
        return new ShakePower((float)v);
    }
}