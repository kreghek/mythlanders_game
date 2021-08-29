using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class HitState : IUnitStateEngine
    {
        private readonly AttackInteraction _attackInteraction;

        private double _counter;

        public HitState(AttackInteraction attackInteraction)
        {
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
            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > 1)
            {
                IsComplete = true;
            }
            else if (_counter > 0.5)
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
