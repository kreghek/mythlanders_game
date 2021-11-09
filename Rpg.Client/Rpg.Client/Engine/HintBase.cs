using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal abstract class HintBase
    {
        private readonly Texture2D _texture;
        private static readonly Rectangle[,] _sourceRects = new[,] {
            { new Rectangle(0, 0, 8,8),
            new Rectangle(8, 0, 16,8),
            new Rectangle(24, 0, 8,8) },

            { new Rectangle(0, 8, 8,16),
            new Rectangle(8, 8, 16,16),
            new Rectangle(24, 8, 8,16) },

            { new Rectangle(0, 24, 8,8),
            new Rectangle(8, 24, 16,8),
            new Rectangle(24, 24, 8,8) }
        };

        protected HintBase(Texture2D texture)
        {
            _texture = texture;
        }

        public Rectangle Rect { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.Lerp(Color.Transparent, Color.SlateBlue, 0.75f);

            var destRectsBase = new[,] {
            { new Rectangle(0, 0, 8,8),
            new Rectangle(8, 0, Rect.Width - (8 * 2),8),
            new Rectangle(Rect.Width - (8 * 2) + 8, 0, 8,8) },

            { new Rectangle(0, 8, 8,Rect.Height - (8 * 2)),
            new Rectangle(8, 8, Rect.Width - (8 * 2),Rect.Height - (8 * 2)),
            new Rectangle(Rect.Width - (8 * 2) + 8, 8, 8,Rect.Height - (8 * 2)) },

            { new Rectangle(0, Rect.Height - (8 * 2), 8,8),
            new Rectangle(8, Rect.Height - (8 * 2), Rect.Width - (8 * 2),8),
            new Rectangle(Rect.Width - (8 * 2) + 8, Rect.Height - (8 * 2), 8,8) }
        };

            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    var destRect = new Rectangle((destRectsBase[x, y].Location.ToVector2() + Rect.Location.ToVector2()).ToPoint(), destRectsBase[x, y].Size);
                    spriteBatch.Draw(_texture, destRect, _sourceRects[x, y], color);
                }
            }

            DrawContent(spriteBatch, Rect);
        }

        protected abstract void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect);
    }
}