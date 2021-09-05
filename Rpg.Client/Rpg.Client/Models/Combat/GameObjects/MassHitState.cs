using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class MassHitState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly IEnumerable<AttackInteraction> _attackInteractions;
        private readonly UnitGraphics _graphics;

        private double _counter;

        private bool _interactionExecuted;

        public MassHitState(UnitGraphics graphics, IEnumerable<AttackInteraction> attackInteractions)
        {
            _graphics = graphics;

            _attackInteractions = attackInteractions;
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
                    _interactionExecuted = true;

                    foreach (var attackInteraction in _attackInteractions)
                    {
                        attackInteraction.Execute();
                    }
                }
            }
        }
    }
}