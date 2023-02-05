using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    public class SpritesTile
    {
        public SpritesTile(Texture2D content, int columns, int rows, int count)
        {
            Content = content;
            Columns = columns;
            Rows = rows;
            Count = count;
        }

        public int Columns { get; }

        //public SpritesTile()
        public Texture2D Content { get; }

        public int Count { get; }
        public int Rows { get; }

        public Texture2D GetSprite(int number)
        {
            if (number > Count)
            {
                return null;
            }

            var width = Content.Width / Columns;
            var height = Content.Height / Rows;

            return null;
        }
    }
}