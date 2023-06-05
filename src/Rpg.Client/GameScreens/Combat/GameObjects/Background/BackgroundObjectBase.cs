using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
    internal abstract class BackgroundObjectBase
    {
        protected BackgroundObjectBase(Texture2D spriteAtlas, Rectangle sourceRectangle)
        {
            Size = 4;
            SpriteAtlas = spriteAtlas;
            SourceRectangle = sourceRectangle;
        }

        public int Size { get; }
        public Rectangle SourceRectangle { get; }
        public Texture2D SpriteAtlas { get; }
    }
}