using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
<<<<<<< HEAD:Rpg.Client/Rpg.Client/Models/Combat/Ui/HpChangedIndicator.cs
    internal class HpChangedIndicator : EwarDrawableComponentBase
=======
    internal abstract class DisapearingTextComponent : EwarDrawableComponentBase
>>>>>>> remotes/origin/combat-bg:Rpg.Client/Rpg.Client/Models/Combat/Ui/DisapearingTextComponent.cs
    {
        private readonly Vector2 _speed;
        private int _lifetime;
        private Vector2 _position;

<<<<<<< HEAD:Rpg.Client/Rpg.Client/Models/Combat/Ui/HpChangedIndicator.cs
        public HpChangedIndicator(EwarGame game, int amount, Vector2 startPosition) : base(game)
=======
        public DisapearingTextComponent(EwarGame game, Vector2 startPosition) : base(game)
>>>>>>> remotes/origin/combat-bg:Rpg.Client/Rpg.Client/Models/Combat/Ui/DisapearingTextComponent.cs
        {
            _position = startPosition;
            _lifetime = 2000;
            _speed = new(0, 10f / 1000f);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var ui = Game.Services.GetService<IUiContentStorage>();

<<<<<<< HEAD:Rpg.Client/Rpg.Client/Models/Combat/Ui/HpChangedIndicator.cs
            spriteBatch.DrawString(ui.GetMainFont(), $"{_amount} HP", _position, _color);
=======
            spriteBatch.DrawString(ui.GetMainFont(), GetText(), _position, GetColor());
>>>>>>> remotes/origin/combat-bg:Rpg.Client/Rpg.Client/Models/Combat/Ui/DisapearingTextComponent.cs

            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (_lifetime <= 0)
            {
                Remove();
                return;
            }

            var elapsed = gameTime.ElapsedGameTime.Milliseconds;

            _position += _speed * elapsed;

            _lifetime -= elapsed;

            base.Update(gameTime);
        }

        protected abstract Color GetColor();

        protected abstract string GetText();
    }
}