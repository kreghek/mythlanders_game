using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal sealed class FireParticleGenerator : IParticleGenerator
    {
        private readonly Random _random;
        private readonly IList<Texture2D> _textures;

        public FireParticleGenerator(IList<Texture2D> textures)
        {
            _textures = textures;
            _random = new Random();
        }

        public IParticle GenerateNewParticle(Vector2 emitterPosition)
        {
            var texture = _textures[_random.Next(_textures.Count)];

            var velocity = new Vector2(
                0.1f * (float)(_random.NextDouble() * 2 - 1),
                2f * (float)(_random.NextDouble() * 4 - 1));
            const float ANGLE = 0;
            const int ANGULAR_VELOCITY = 0;
            var color = Color.Lerp(Color.White, Color.Transparent, 0.5f);
            var size = 0.2f + (float)_random.NextDouble() * 0.1f;
            var ttl = 100 + _random.Next(100);

            return new SwarmParticle(texture, new Rectangle(0, 32*2, 32, 32), emitterPosition, velocity, ANGLE, ANGULAR_VELOCITY, Color.Purple,
                size, ttl);
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
}