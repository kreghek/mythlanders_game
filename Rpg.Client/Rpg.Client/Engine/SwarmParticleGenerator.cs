using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal sealed class SwarmParticleGenerator : IParticleGenerator
    {
        private readonly Random _random;
        private readonly IList<Texture2D> _textures;

        public SwarmParticleGenerator(IList<Texture2D> textures)
        {
            _textures = textures;
            _random = new Random();
        }

        public IParticle GenerateNewParticle(Vector2 emitterPosition)
        {
            var texture = _textures[_random.Next(_textures.Count)];
            var position = emitterPosition;
            var velocity = new Vector2(
                1f * (float)(_random.NextDouble() * 2 - 1),
                1f * (float)(_random.NextDouble() * 2 - 1));
            float angle = 0;
            var angularVelocity = 0.1f * (float)(_random.NextDouble() * 2 - 1);
            var color = new Color(
                (float)_random.NextDouble(),
                (float)_random.NextDouble(),
                (float)_random.NextDouble());
            var size = (float)_random.NextDouble();
            var ttl = 20 + _random.Next(40);

            return new Particle(texture, new Rectangle(0, 0, 32, 32), position, velocity, angle, angularVelocity, color,
                size, ttl);
        }

        public int GetCount()
        {
            return 3;
        }

        public float GetTimeout()
        {
            return 0.2f;
        }
    }
}