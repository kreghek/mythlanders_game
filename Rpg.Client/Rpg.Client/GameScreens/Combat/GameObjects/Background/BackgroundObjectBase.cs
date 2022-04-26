namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
    internal abstract class BackgroundObjectBase
    {
        public int Size { get; }
        public Texture2D SpriteAtlas { get; }
        public Rectangle SourceRectangle { get; }

        protected BackgroundObjectBase(Texture2D spriteAtlas, Rectangle sourceRectangle)
        {
            Size = 4;
            SpriteAtlas = spriteAtlas;
            SourceRectangle = sourceRectangle;
        }
    }
}