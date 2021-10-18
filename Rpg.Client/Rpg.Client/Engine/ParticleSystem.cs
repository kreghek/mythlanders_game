using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal class ParticleSystem
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private IList<Particle> particles;
        private IList<Texture2D> textures;

        public ParticleSystem(IList<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            this.textures = textures;
            particles = new List<Particle>();
            random = new Random();
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

        private Particle GenerateNewParticle()
        {
            var texture = textures[random.Next(textures.Count)];
            var position = EmitterLocation;
            var velocity = new Vector2(
                                    1f * (float)(random.NextDouble() * 2 - 1),
                                    1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            var angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            var color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            var size = (float)random.NextDouble();
            var ttl = 20 + random.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            for (var index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            //spriteBatch.End();
        }
    }
}