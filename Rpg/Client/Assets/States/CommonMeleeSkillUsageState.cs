using System;

using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States
{
    internal sealed class CommonMeleeSkillUsageState : IActorVisualizationState
    {
        private readonly AnimationBlocker _blocker;
        private readonly IActorVisualizationState _innerState;

        public CommonMeleeSkillUsageState(UnitGraphics graphics, SpriteContainer graphicsRoot,
            Renderable targetGraphicsRoot,
            AnimationBlocker mainAnimationBlocker, SkillAnimationInfo animationInfo,
            PredefinedAnimationSid animationSid)
        {
            //var targetPosition =
            //    targetGraphicsRoot.Position + new Vector2(-100 * (targetGraphicsRoot.FlipX ? 1 : -1), 0);

            //var subStates = new IActorVisualizationState[]
            //{
            //    new LinearMoveToTargetState(graphics, graphicsRoot, targetPosition, animationSid),
            //    new DirectInteractionState(graphics, animationInfo, animationSid),
            //    new LinearMoveBackState(graphics, graphicsRoot, targetPosition, mainAnimationBlocker)
            //};

            //_innerState = new SequentialState(subStates);

            //_blocker = mainAnimationBlocker;

            throw new Exception();
        }

        public bool CanBeReplaced => _innerState.CanBeReplaced;
        public bool IsComplete => _innerState.IsComplete;

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
            _innerState.Update(gameTime);
        }
    }
}