using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal class ParticleSystem
    {
        private readonly IList<IParticle> _particles;
        private readonly IParticleGenerator _particleGenerator;

        public ParticleSystem(Vector2 location, IParticleGenerator particleGenerator)
        {
            EmitterLocation = location;
            _particleGenerator = particleGenerator;
            _particles = new List<IParticle>();
        }

        private Vector2 EmitterLocation { get; }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var particle in _particles)
            {
                particle.Draw(spriteBatch);
            }
        }

        private double _updateCounter;
        public void Update(GameTime gameTime)
        {
            _updateCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (_updateCounter > 0.1)
            {
                _updateCounter = 0;
                return;
            }

            var total = 3;

            for (var i = 0; i < total; i++)
            {
                _particles.Add(GenerateNewParticle());
            }

            for (var particle = 0; particle < _particles.Count; particle++)
            {
                _particles[particle].Update();
                if (_particles[particle].TTL <= 0)
                {
                    _particles.RemoveAt(particle);
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