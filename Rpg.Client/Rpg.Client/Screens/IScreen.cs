using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Screens
{
    internal interface IScreen
    {
        IScreen? TargetScreen { get; set; }

        void Draw(SpriteBatch spriteBatch);

        void Update(GameTime gameTime);
    }
}