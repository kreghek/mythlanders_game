using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class HitState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly UnitGraphics _graphics;
        private readonly AttackInteraction _attackInteraction;

        private double _counter;

        public HitState(UnitGraphics graphics, AttackInteraction attackInteraction)
        {
            _graphics = graphics;
            _attackInteraction = attackInteraction;
        }

        public bool CanBeReplaced { get; }
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            throw new System.NotImplementedException();
        }

        private bool _interactionExecuted;

        public void Update(GameTime gameTime)
        {
            if (_counter == 0)
            {
                _graphics.PlayAnimation("Hit");
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > DURATION)
            {
                IsComplete = true;
            }
            else if (_counter > DURATION / 2)
            {
                if (!_interactionExecuted)
                {
                    _attackInteraction.Execute();

                    _interactionExecuted = true;
                }
            }
        }
    }
}
