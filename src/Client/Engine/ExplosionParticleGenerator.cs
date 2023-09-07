using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine;

internal sealed class ExplosionParticleGenerator : IParticleGenerator
{
    private readonly Random _random;
    private readonly Rectangle _sourceTextureRect;
    private readonly IList<Texture2D> _textures;

    public ExplosionParticleGenerator(IList<Texture2D> textures, Rectangle sourceTextureRect)
    {
        _textures = textures;
        _sourceTextureRect = sourceTextureRect;
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
        var randomVector = new Vector2(randomUnitVector.X, randomUnitVector.Y * 0.3f) * 1280;
        var startPosition = emitterPosition;
        var targetPosition = emitterPosition + randomVector;

        var velocity = new Vector2(
            1f * (float)(_random.NextDouble() * 20 - 1),
            1f * (float)(_random.NextDouble() * 20 - 1));
        var angle = 0f;
        var angularVelocity = 0.1f * (float)(_random.NextDouble() * 2 - 1);
        var color = Color.White;
        var size = (float)_random.NextDouble() * 5f;
        var ttl = 40 + _random.Next(40);

        return new MothParticle(texture, _sourceTextureRect, startPosition, targetPosition, velocity,
            angle, angularVelocity, color, size, ttl);
    }

    public int GetCount()
    {
        return 15;
    }

    public float GetTimeout()
    {
        return 0.1f;
    }
}