using System;

using Microsoft.Xna.Framework;

namespace Client.Engine.PostProcessing;

public sealed class ConstantShakePostEffect : IPostEffect
{
    private readonly ShakePower _power;
    private readonly Random _random;
    private Vector2 _shakeVector;

    public ConstantShakePostEffect(ShakePower power)
    {
        _power = power;

        _random = new Random();
    }

    public void Apply(PostEffectCatalog postEffectCatalog)
    {
        postEffectCatalog.ShakeEffect.Parameters["DistanceX"].SetValue(_shakeVector.X);
        postEffectCatalog.ShakeEffect.Parameters["DistanceY"].SetValue(_shakeVector.Y);

        foreach (var technique in postEffectCatalog.ShakeEffect.Techniques)
        {
            foreach (var pass in technique.Passes)
            {
                pass.Apply();
            }
        }
    }

    public void Update(GameTime gameTime, PostEffectCatalog postEffectCatalog)
    {
        _shakeVector = Vector2.One * _random.NextSingle() * _power.Value;
    }
}