using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal interface IParticleGenerator
    {
        IParticle GenerateNewParticle(Vector2 emitterPosition);
    }

    internal sealed class SwarmParticleGenerator: IParticleGenerator
    {
        private readonly IList<Texture2D> _textures;
        private readonly Random _random;

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

            return new Particle(texture, new Rectangle(0, 0, 32, 32), position, velocity, angle, angularVelocity, color, size, ttl);
        }
    }
    
    internal sealed class MothParticleGenerator: IParticleGenerator
    {
        private readonly IList<Texture2D> _textures;
        private readonly Random _random;

        public MothParticleGenerator(IList<Texture2D> textures)
        {
            _textures = textures;
            _random = new Random();
        }
        
        public IParticle GenerateNewParticle(Vector2 emitterPosition)
        {
            var texture = _textures[_random.Next(_textures.Count)];

            var randomUnitVector = CreateRandomUnitVector2((float)(Math.PI * 2), 0);
            var randomVector = randomUnitVector * 128;
            var startPosition = emitterPosition + randomVector;
            
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

            return new MothParticle(texture, new Rectangle(0, 0, 32, 32), startPosition, emitterPosition, velocity, angle, angularVelocity, color, size, ttl);
        }
        
        private Vector2 CreateRandomUnitVector2(float angle, float angleMin){
            float random = _random.Next() * angle + angleMin;
            return new Vector2((float)Math.Cos(random), (float)Math.Sin(random));
        }
    }

    internal class ParticleSystem
    {
        private readonly IList<IParticle> particles;
        private readonly IList<Texture2D> textures;
        private readonly IParticleGenerator _particleGenerator;

        public ParticleSystem(Vector2 location, IParticleGenerator particleGenerator)
        {
            EmitterLocation = location;
            _particleGenerator = particleGenerator;
            particles = new List<IParticle>();
        }

        public Vector2 EmitterLocation { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            for (var index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            //spriteBatch.End();
        }

        public void Update()
        {
            var total = 3;

            for (var i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }

            for (var particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private IParticle GenerateNewParticle()
        {
            return _particleGenerator.GenerateNewParticle(EmitterLocation);
        }
    }
}