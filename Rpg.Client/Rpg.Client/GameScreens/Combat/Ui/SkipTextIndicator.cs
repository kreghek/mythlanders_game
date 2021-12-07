using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed class SkipTextIndicator : TextIndicatorBase
    {
        public SkipTextIndicator(Vector2 startPosition, SpriteFont font) : base(startPosition, font)
        {
        }

        protected override Color GetColor()
        {
            return Color.LightGray;
        }

        protected override string GetText()
        {
            return UiResource.IndicatorSkipTurn;
        }
    }
}