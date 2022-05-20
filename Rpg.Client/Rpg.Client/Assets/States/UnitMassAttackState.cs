using System;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;
using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates.Primitives;

namespace Rpg.Client.Assets.States
{
    internal class UnitMassAttackState : IUnitStateEngine
    {
        private readonly AnimationBlocker _blocker;
        private readonly UnitGraphics _graphics;
        private readonly IUnitStateEngine[] _subStates;

        private int _subStateIndex;

        public UnitMassAttackState(UnitGraphics graphics, SpriteContainer graphicsRoot,
            SpriteContainer targetGraphicsRoot,
            AnimationBlocker blocker, Action attackInteractions, PredefinedAnimationSid animationSid)
        {
            var targetPosition =
                targetGraphicsRoot.Position + new Vector2(-100 * (targetGraphicsRoot.FlipX ? 1 : -1), 0);
            _subStates = new IUnitStateEngine[]
            {
                new LinearMoveToTargetState(graphics, graphicsRoot, targetPosition, animationSid),
                new MassHitState(graphics, attackInteractions, animationSid),
                new LinearMoveBackState(graphics, graphicsRoot, targetPosition, blocker)
            };
            _graphics = graphics;
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
    }
}