using Microsoft.Xna.Framework;

namespace Rpg.Client.Models.Combat.Ui
{
    internal class HpChangedComponent : DisapearingTextComponent
    {
        private readonly int _amount;

        public HpChangedComponent(EwarGame game,int amount, Vector2 startPosition) : base(game, startPosition)
        {
            _amount = amount;
        }
        protected override string GetText()
        {
            return $"{_amount} HP";
        }

        protected override Color GetColor()
        {
            return _amount >= 0 ? Color.LightGreen : Color.Red;
        }
    }
}