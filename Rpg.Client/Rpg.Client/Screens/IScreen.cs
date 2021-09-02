using Microsoft.Xna.Framework;

namespace Rpg.Client.Screens
{
    internal interface IScreen
    {
        IScreen? TargetScreen { get; set; }
        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);
    }
}