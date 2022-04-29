using System;

using Microsoft.Xna.Framework;

using Rpg.Client.Assets.States.Primitives;
using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;
using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates;
using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates.Primitives;

namespace Rpg.Client.Assets.States
{
    internal sealed class CommonMeleeSkillUsageState : IUnitStateEngine
    {
        private readonly AnimationBlocker _blocker;
        private readonly IUnitStateEngine[] _subStates;

        private int _subStateIndex;

        public CommonMeleeSkillUsageState(UnitGraphics graphics, SpriteContainer graphicsRoot,
            SpriteContainer targetGraphicsRoot,
            AnimationBlocker blocker, SkillAnimationInfo animationInfo, AnimationSid animationSid)
        {
            var targetPosition =
                targetGraphicsRoot.Position + new Vector2(-100 * (targetGraphicsRoot.FlipX ? 1 : -1), 0);

            _subStates = new IUnitStateEngine[]
            {
                new LinearMoveToTargetState(graphics, graphicsRoot, targetPosition, animationSid),
                new DirectInteractionState(graphics, animationInfo, animationSid),
                new LinearMoveBackState(graphics, graphicsRoot, targetPosition, blocker)
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
                Completed?.Invoke(this, EventArgs.Empty);
                IsComplete = true;
            }
        }

        public event EventHandler? Completed;
    }
}