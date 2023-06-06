using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine;

internal sealed class HorizontalPulseParticleGenerator : IParticleGenerator
{
    private readonly Random _random;
    private readonly IList<Texture2D> _textures;

    public HorizontalPulseParticleGenerator(IList<Texture2D> textures)
    {
        _textures = textures;
        _random = new Random();
    }

    private Vector2 CreateRandomUnitVector2(float angle, float angleMin)
    {
        var random = _random.Next() * angle + angleMin;
        return new Vector2((float)Math.Cos(random), (float)Math.Sin(random));
    }

    public IParticle GenerateNewParticle(Vector2 emitterPosition)
    {
        var texture = _textures[_random.Next(_textures.Count)];

        var randomUnitVector = CreateRandomUnitVector2((float)(Math.PI * 2), 0);
        var randomVector = new Vector2(randomUnitVector.X, 0) * 1280;
        var startPosition = emitterPosition;
        var targetPosition = startPosition + randomVector;

        var velocity = new Vector2(
            1f * (float)(_random.NextDouble() * 1 - 1),
            0);
        var angle = 0f;
        var angularVelocity = 0.1f * (float)(_random.NextDouble() * 2 - 1);
        var color = new Color(145, 204, 202, 127);
        var size = (float)_random.NextDouble() * 2.5f;
        var ttl = 1200 + _random.Next(20);

        return new PulseItemParticle(texture, new Rectangle(0, 64 + 32, 32, 32), startPosition, targetPosition,
            velocity,
            angle, angularVelocity, color, size, ttl);
    }

    public int GetCount()
    {
        return 15;
    }

    public float GetTimeout()
    {
        return 0.666f;
    }
}

internal sealed class HorizontalPulseParticleGenerator2 : IParticleGenerator
{
    private readonly Random _random;
    private readonly IList<Texture2D> _textures;

    public HorizontalPulseParticleGenerator2(IList<Texture2D> textures)
    {
        _textures = textures;
        _random = new Random();
    }

    private Vector2 CreateRandomUnitVector2(float angle, float angleMin)
    {
        var random = _random.Next() * angle + angleMin;
        return new Vector2((float)Math.Cos(random), (float)Math.Sin(random));
    }

    public IParticle GenerateNewParticle(Vector2 emitterPosition)
    {
        var texture = _textures[_random.Next(_textures.Count)];

        var randomUnitVector = CreateRandomUnitVector2((float)(Math.PI * 2), 0);
        var randomVector = new Vector2(randomUnitVector.X, 0) * 1280;
        var startPosition = emitterPosition;
        var targetPosition = startPosition - new Vector2(0, 80) + randomVector;

        var velocity = new Vector2(
            1f * (float)(_random.NextDouble() * 1 - 1),
            0);
        var angle = 0f;
        var angularVelocity = 0.1f * (float)(_random.NextDouble() * 2 - 1);
        var color = new Color(145, 204, 202, 127);
        var size = (float)_random.NextDouble() * 0.5f;
        var ttl = 1200 + _random.Next(20);

        return new PulseItemParticle(texture, new Rectangle(0, 64 + 32, 32, 32), startPosition, targetPosition,
            velocity,
            angle, angularVelocity, color, size, ttl);
    }

    public int GetCount()
    {
        return 5;
    }

    public float GetTimeout()
    {
        return 0.1f;
    }
}