using System;

using Microsoft.Xna.Framework;

using Rpg.Client.Assets.States.Primitives;
using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;
using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates;

namespace Rpg.Client.Assets.States
{
    internal class UnitMeleeAttackState : IUnitStateEngine
    {
        private readonly AnimationBlocker _blocker;
        private readonly IUnitStateEngine[] _subStates;

        private int _subStateIndex;

        public UnitMeleeAttackState(UnitGraphics graphics, SpriteContainer graphicsRoot,
            SpriteContainer targetGraphicsRoot,
            AnimationBlocker blocker, SkillAnimationInfo animationInfo, AnimationSid animationSid)
        {
            var targetPosition =
                targetGraphicsRoot.Position + new Vector2(-100 * (targetGraphicsRoot.FlipX ? 1 : -1), 0);
            _subStates = new IUnitStateEngine[]
            {
                new MoveToTarget(graphics, graphicsRoot, targetPosition, animationSid),
                new DirectInteractionState(graphics, animationInfo, animationSid),
                new MoveBack(graphics, graphicsRoot, targetPosition, blocker)
            };
            _blocker = blocker;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

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

        public event EventHandler? Completed;
    }
}