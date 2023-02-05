using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.GameScreens.SlidingPuzzles
{
    internal sealed class PuzzleButton : ButtonBase
    {
        private readonly int _columns;
        private readonly Texture2D _puzzleTexture;
        private readonly int _size;

        public PuzzleButton(Texture2D puzzleTexture, int columns)
        {
            _puzzleTexture = puzzleTexture;
            _columns = columns;
            _size = puzzleTexture.Width / columns;
        }

        public int Number { get; set; }

        protected override int Margin => 0;

        protected override Point CalcTextureOffset()
        {
            return Number > 0 ? ControlTextures.PanelBlack : ControlTextures.Transparent;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            if (Number > 0)
            {
                var column = Number % _columns;
                var row = Number / _columns;

                var sourceRect = new Rectangle(column * _size, row * _size, _size, _size);

                spriteBatch.Draw(_puzzleTexture, contentRect, sourceRect, color);
                spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetTitlesFont(), Number.ToString(),
                    contentRect.Center.ToVector2(), Color.Red);
            }
        }

        protected override void UpdateContent()
        {
            base.UpdateContent();

            IsEnabled = Number > 0;
        }
    }
}