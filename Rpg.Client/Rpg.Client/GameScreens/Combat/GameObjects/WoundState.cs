using System;

using Microsoft.Xna.Framework;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class WoundState : IUnitStateEngine
    {
        private readonly UnitGraphics _graphics;
        private double _counter;

        public WoundState(UnitGraphics graphics)
        {
            _graphics = graphics;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            if (_counter == 0)
            {
                _graphics.PlayAnimation(Core.AnimationSid.Wound);
                _graphics.IsDamaged = true;
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > 0.05)
            {
                _graphics.IsDamaged = false;
            }

            if (_counter > 1)
            {
                _graphics.IsDamaged = false;
                IsComplete = true;
            }
        }
    }
}