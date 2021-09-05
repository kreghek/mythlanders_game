using System;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class HealState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly HealInteraction _healInteraction;
        private readonly UnitGraphics _graphics;

        private double _counter;

        private bool _interactionExecuted;

        public HealState(UnitGraphics graphics, HealInteraction healInteraction)
        {
            _graphics = graphics;
            _healInteraction = healInteraction;
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
                    _healInteraction.Execute();

                    _interactionExecuted = true;
                }
            }
        }
    }
}