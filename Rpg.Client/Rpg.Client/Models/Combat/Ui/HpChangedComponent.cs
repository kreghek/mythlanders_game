using Microsoft.Xna.Framework;

namespace Rpg.Client.Models.Combat.Ui
{
    internal class HpChangedComponent : DisapearingTextComponent
    {
        private readonly int _amount;

        public HpChangedComponent(int amount, Vector2 startPosition) : base(startPosition)
        {
            _amount = amount;
        }

        protected override Color GetColor()
        {
            return _amount >= 0 ? Color.LightGreen : Color.Red;
        }

        protected override string GetText()
        {
            return $"{_amount} HP";
        }
    }
}