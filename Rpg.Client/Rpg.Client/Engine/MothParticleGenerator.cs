using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal sealed class MothParticleGenerator : IParticleGenerator
    {
        private readonly Random _random;
        private readonly IList<Texture2D> _textures;
        private readonly float _radius;

        public MothParticleGenerator(IList<Texture2D> textures, float radius = 128f)
        {
            _textures = textures;
            _radius = radius;
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
            var randomVector = new Vector2(randomUnitVector.X, randomUnitVector.Y * 0.3f) * _radius;
            var startPosition = emitterPosition + randomVector;

            var velocity = new Vector2(
                1f * (float)(_random.NextDouble() * 2 - 1),
                1f * (float)(_random.NextDouble() * 2 - 1));
            const float ANGLE = 0;
            const int ANGULAR_VELOCITY = 0;
            var color = Color.Lerp(Color.Yellow, Color.Transparent, 0.5f);
            var size = (float)(0.5f + _random.NextDouble());
            var ttl = 20 + _random.Next(40);

            return new MothParticle(texture, new Rectangle(0, 32, 32, 32), startPosition, emitterPosition, velocity,
                ANGLE, ANGULAR_VELOCITY, color, size, ttl);
        }

        public int GetCount()
        {
            return 3;
        }

        public float GetTimeout()
        {
            return 0.5f;
        }
    }
}