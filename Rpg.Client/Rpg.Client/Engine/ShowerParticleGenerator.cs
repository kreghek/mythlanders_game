using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal sealed class ShowerParticleGenerator : IParticleGenerator
    {
        private readonly Random _random;
        private readonly IList<Texture2D> _textures;

        public ShowerParticleGenerator(IList<Texture2D> textures)
        {
            _textures = textures;
            _random = new Random();
        }

        public IParticle GenerateNewParticle(Vector2 emitterPosition)
        {
            var texture = _textures[_random.Next(_textures.Count)];
            var position = emitterPosition;
            var targetPosition = emitterPosition + Vector2.UnitY * -32;
            var velocity = new Vector2(
                1f * (float)(_random.NextDouble() * 2 - 1),
                1f * (float)(_random.NextDouble() * 2 - 1));
            float angle = 0;
            var angularVelocity = 0;
            var color = Color.LimeGreen;
            var size = 0.5f;
            var ttl = 20 + _random.Next(40);

            return new MothParticle(texture, new Rectangle(0, 0, 32, 32), position, targetPosition, velocity, angle, angularVelocity,
                color,
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