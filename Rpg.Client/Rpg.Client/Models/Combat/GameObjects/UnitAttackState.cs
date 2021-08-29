using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal class UnitAttackState : IUnitStateEngine
    {
        private IUnitStateEngine[] _subStates;

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public UnitAttackState(SpriteContainer graphicsRoot, SpriteContainer targetGraphicsRoot, AnimationBlocker blocker, AttackInteraction attackInteraction)
        {
            _subStates = new IUnitStateEngine[]
                {
                    new MoveToTarget(graphicsRoot, targetGraphicsRoot),
                    new HitState(attackInteraction),
                    new MoveBack(graphicsRoot, targetGraphicsRoot, blocker)
                };
            _blocker = blocker;
        }

        public void Cancel()
        {
            if (IsComplete)
            {
                return;
            }

            _blocker.Release();
        }

        public void Update(GameTime gameTime)
        {
            if (_subStateIndex < _subStates.Length)
            {
                var currentSubState = _subStates[_subStateIndex];
                if (currentSubState.IsComplete)
                {
                    _subStateIndex++;
                }
                else
                {
                    currentSubState.Update(gameTime);
                }
            }
            else
            {
                IsComplete = true;
            }
        }

        private int _subStateIndex = 0;
        private readonly AnimationBlocker _blocker;
    }
}
