using Microsoft.Xna.Framework;

namespace Rpg.Client.Models.Combat.Ui
{
    internal class MovePassedComponent : DisapearingTextComponent
    {
        public MovePassedComponent(Vector2 startPosition) : base(startPosition)
        {
        }

        protected override Color GetColor()
        {
            return Color.LightGray;
        }

        protected override string GetText()
        {
            return "Passed";
        }
    }
}