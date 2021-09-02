using Microsoft.Xna.Framework;

namespace Rpg.Client.Screens
{
    interface IScreen
    {
        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);
        IScreen? TargetScreen { get; set; }
    }
}