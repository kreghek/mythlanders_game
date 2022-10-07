using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
    internal sealed class WeatherAnimatedObject : IBackgroundObject
    {
        private readonly ParticleSystem _particleSystem;

        public WeatherAnimatedObject(Texture2D texture, Rectangle sourceRect)
        {
            var particleGenerator = new WeatherParticleGenerator(new[] { texture });
            _particleSystem = new ParticleSystem(Vector2.Zero, particleGenerator);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _particleSystem.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _particleSystem.Update(gameTime);
        }
    }
}