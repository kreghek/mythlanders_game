using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal interface IInteractionDelivery
    {
        bool IsDestroyed { get; }

        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}