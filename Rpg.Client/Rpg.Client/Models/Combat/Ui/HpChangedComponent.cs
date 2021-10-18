using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Models.Combat.Ui
{
    internal class HpChangedComponent : DisapearingTextComponent
    {
        private readonly int _amount;

        public HpChangedComponent(int amount, Vector2 startPosition, SpriteFont font) : base(startPosition, font)
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