using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
    internal sealed class StaticBackgroundObject : BackgroundObjectBase
    {
        public StaticBackgroundObject(Texture2D spriteAtlas, Rectangle sourceRectangle) : base(spriteAtlas,
            sourceRectangle)
        {
        }
    }
}