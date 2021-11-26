using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal sealed class TailParticleGenerator : IParticleGenerator
    {
        private readonly Random _random;
        private readonly IList<Texture2D> _textures;

        public TailParticleGenerator(IList<Texture2D> textures)
        {
            _textures = textures;
            _random = new Random();
        }

        public IParticle GenerateNewParticle(Vector2 emitterPosition)
        {
            var texture = _textures[_random.Next(_textures.Count)];

            var velocity = new Vector2(
                2f * (float)(_random.NextDouble() * 2 - 1),
                2f * (float)(_random.NextDouble() * 2 - 1));
            const float ANGLE = 0;
            const int ANGULAR_VELOCITY = 0;
            var color = Color.Lerp(Color.LightCyan, Color.Transparent, 0.5f);
            var size = 0.1f;
            var ttl = 10 + _random.Next(10);

            return new SwarmParticle(texture, new Rectangle(0, 32, 32, 32), emitterPosition, velocity,
                ANGLE, ANGULAR_VELOCITY, color, size, ttl);
        }

        public int GetCount()
        {
            return 30;
        }

        public float GetTimeout()
        {
            return 0.05f;
        }
    }
}