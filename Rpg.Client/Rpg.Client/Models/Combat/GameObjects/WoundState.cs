using System;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class WoundState : IUnitStateEngine
    {
        private readonly UnitGraphics _graphics;
        private double _counter;

        public WoundState(UnitGraphics graphics)
        {
            _graphics = graphics;
        }

        public bool CanBeReplaced { get; }
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            if (_counter == 0)
            {
                _graphics.PlayAnimation("Wound");
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > 1)
            {
                IsComplete = true;
            }
        }
    }
}