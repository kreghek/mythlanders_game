using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
    internal sealed class DemonicFireAnimatedObject : IBackgroundObject
    {
        private readonly ParticleSystem _particleSystem;

        public DemonicFireAnimatedObject(Texture2D texture, Rectangle sourceRect, Vector2 position)
        {
            var particleGenerator = new FireParticleGenerator(new[] { texture });
            _particleSystem = new ParticleSystem(Vector2.Zero, particleGenerator);
            _particleSystem.MoveEmitter(position);
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