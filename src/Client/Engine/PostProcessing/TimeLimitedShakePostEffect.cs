using System;

using GameClient.Engine;

using Microsoft.Xna.Framework;

namespace Client.Engine.PostProcessing;

public sealed class TimeLimitedShakePostEffect : IPostEffect
{
    private readonly Duration _duration;
    private readonly IShakeFunction _func;
    private readonly Random _random;

    private double _durationCounter;
    private Vector2 _shakeVector;

    public TimeLimitedShakePostEffect(Duration duration, IShakeFunction func)
    {
        _duration = duration;
        _func = func;

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
        if (_durationCounter < _duration.Seconds)
        {
            _durationCounter += gameTime.ElapsedGameTime.TotalSeconds;

            var actualT = _durationCounter / _duration.Seconds;
            _shakeVector = Vector2.One * _random.NextSingle() * _func.Calculate(actualT).Value;
        }
        else
        {
            _shakeVector = Vector2.One * _random.NextSingle() * _func.Calculate(1).Value;
        }
    }
}