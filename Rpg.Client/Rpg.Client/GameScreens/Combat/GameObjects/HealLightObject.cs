using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class HealLightObject : IInteractionDelivery
    {
        private const double DURATION_SECONDS = 1.0;
        private readonly AnimationBlocker? _blocker;
        private readonly ParticleSystem _particleSystem;
        private double _counter;

        public HealLightObject(Vector2 targetPosition, GameObjectContentStorage contentStorage,
            AnimationBlocker? blocker)
        {
            var swarmParticleGenerator = new SwarmParticleGenerator(new[] { contentStorage.GetParticlesTexture() });
            _particleSystem = new ParticleSystem(targetPosition, swarmParticleGenerator);

            _blocker = blocker;
        }

        public bool IsDestroyed { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
            {
                return;
            }

            _particleSystem.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (IsDestroyed)
            {
                return;
            }

            if (_counter < DURATION_SECONDS)
            {
                _counter += gameTime.ElapsedGameTime.TotalSeconds;

                _particleSystem.Update(gameTime);
            }
            else
            {
                if (IsDestroyed)
                {
                    return;
                }

                IsDestroyed = true;
                _blocker?.Release();
            }
        }
    }
}