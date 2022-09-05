using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    public abstract class ControlBase
    {
        public const int CONTENT_MARGIN = 4;
        private const int CORNER_SIZE = 15;
        private const int INNER_SIZE = (16 - CORNER_SIZE) * 2;

        private static readonly Rectangle[,] _sourceRects =
        {
            {
                new(0, 0, CORNER_SIZE, CORNER_SIZE),
                new(CORNER_SIZE, 0, INNER_SIZE, CORNER_SIZE),
                new(CORNER_SIZE + INNER_SIZE, 0, CORNER_SIZE, CORNER_SIZE)
            },

            {
                new(0, CORNER_SIZE, CORNER_SIZE, INNER_SIZE),
                new(CORNER_SIZE, CORNER_SIZE, INNER_SIZE, INNER_SIZE),
                new(INNER_SIZE + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE, INNER_SIZE)
            },

            {
                new(0, INNER_SIZE + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE),
                new(CORNER_SIZE, INNER_SIZE + CORNER_SIZE, INNER_SIZE, CORNER_SIZE),
                new(INNER_SIZE + CORNER_SIZE, INNER_SIZE + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE)
            }
        };

        protected abstract Point CalcTextureOffset();

        public Rectangle Rect { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            var color = CalculateColor();

            DrawBackground(spriteBatch: spriteBatch, color: color);

            var contentRect = new Rectangle(
                CONTENT_MARGIN + Rect.Left,
                CONTENT_MARGIN + Rect.Top,
                Rect.Width - (CONTENT_MARGIN * 2),
                Rect.Height - (CONTENT_MARGIN * 2));

            DrawContent(spriteBatch, contentRect, color);
        }

        protected abstract Color CalculateColor();

        protected virtual void DrawBackground(SpriteBatch spriteBatch, Color color)
        {
            var rectWidth = Rect.Width - (CORNER_SIZE * 2);
            var rectHeight = Rect.Height - (CORNER_SIZE * 2);

            var destRectsBase = new[,]
            {
                {
                    new Rectangle(0, 0, CORNER_SIZE, CORNER_SIZE),
                    new Rectangle(CORNER_SIZE, 0, rectWidth, CORNER_SIZE),
                    new Rectangle(rectWidth + CORNER_SIZE, 0, CORNER_SIZE, CORNER_SIZE)
                },
                {
                    new Rectangle(0, CORNER_SIZE, CORNER_SIZE, rectHeight),
                    new Rectangle(CORNER_SIZE, CORNER_SIZE, rectWidth, rectHeight),
                    new Rectangle(rectWidth + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE, rectHeight)
                },
                {
                    new Rectangle(0, rectHeight + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE),
                    new Rectangle(CORNER_SIZE, rectHeight + CORNER_SIZE, rectWidth, CORNER_SIZE),
                    new Rectangle(rectWidth + CORNER_SIZE, rectHeight + CORNER_SIZE, CORNER_SIZE, CORNER_SIZE)
                }
            };

            var _texture = UiThemeManager.UiContentStorage.GetControlBackgroundTexture();

            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    var sourceRect = _sourceRects[x, y];
                    sourceRect.Offset(CalcTextureOffset());

                    var controlPosition = destRectsBase[x, y].Location + Rect.Location;
                    var destRect = new Rectangle(controlPosition, destRectsBase[x, y].Size);
                    spriteBatch.Draw(_texture, destRect, sourceRect, color);
                }
            }
        }

        protected abstract void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor);
    }
}