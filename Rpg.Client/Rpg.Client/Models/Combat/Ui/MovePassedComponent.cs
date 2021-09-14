using Microsoft.Xna.Framework;

namespace Rpg.Client.Models.Combat.Ui
{
    internal class MovePassedComponent : DisapearingTextComponent
    {
        public MovePassedComponent(EwarGame game, Vector2 startPosition) : base(game, startPosition)
        {
        }

        protected override Color GetColor()
        {
            return Color.DarkSlateGray;
        }

        protected override string GetText()
        {
            return "Passed";
        }
    }
}