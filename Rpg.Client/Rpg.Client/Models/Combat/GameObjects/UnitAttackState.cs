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

        public UnitAttackState(UnitGraphics graphics, SpriteContainer graphicsRoot, SpriteContainer targetGraphicsRoot, AnimationBlocker blocker, AttackInteraction attackInteraction)
        {
            var targetPosition = targetGraphicsRoot.Position + new Vector2(-100 * (targetGraphicsRoot.FlipX ? 1 : -1), 0);
            _subStates = new IUnitStateEngine[]
                {
                    new MoveToTarget(graphics, graphicsRoot, targetPosition),
                    new HitState(graphics, attackInteraction),
                    new MoveBack(graphics, graphicsRoot, targetPosition, blocker)
                };
            _graphics = graphics;
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
        private readonly UnitGraphics _graphics;
        private readonly AnimationBlocker _blocker;
    }
}
