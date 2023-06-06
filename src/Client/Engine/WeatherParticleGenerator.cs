using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.Engine;

internal sealed class WeatherParticleGenerator : IParticleGenerator
{
    private readonly Random _random;
    private readonly IList<Texture2D> _textures;

    public WeatherParticleGenerator(IList<Texture2D> textures)
    {
        _textures = textures;
        _random = new Random();
    }

    public IParticle GenerateNewParticle(Vector2 emitterPosition)
    {
        var texture = _textures[_random.Next(_textures.Count)];

        var startPosition = new Vector2(1000 * (float)_random.NextDouble(), 0);
        var targetPosition = new Vector2(startPosition.X + 200 + _random.Next(400), 480);

        var velocity = new Vector2(
            1f * (float)(_random.NextDouble() * 2 - 1),
            1f * (float)(_random.NextDouble() * 2 - 1));
        const float ANGLE = 0;
        const int ANGULAR_VELOCITY = 0;
        var color = Color.Lerp(Color.White, Color.Transparent, 0.5f);
        var size = 0.1f + (float)_random.NextDouble() * 0.1f;
        var ttl = 100 + _random.Next(100);

        return new SnowParticle(texture, new Rectangle(0, 32, 32, 32), startPosition, targetPosition, velocity,
            ANGLE, ANGULAR_VELOCITY, color, size, ttl, xAmpl: _random.Next(16) + 16);
    }

    public int GetCount()
    {
        return 3;
    }

    public float GetTimeout()
    {
        return 0.15f;
    }
}