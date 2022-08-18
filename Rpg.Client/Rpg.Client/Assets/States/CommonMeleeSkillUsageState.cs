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
        private readonly IUnitStateEngine _innerState;

        public CommonMeleeSkillUsageState(UnitGraphics graphics, SpriteContainer graphicsRoot,
            Renderable targetGraphicsRoot,
            AnimationBlocker mainAnimationBlocker, SkillAnimationInfo animationInfo,
            PredefinedAnimationSid animationSid)
        {
            var targetPosition =
                targetGraphicsRoot.Position + new Vector2(-100 * (targetGraphicsRoot.FlipX ? 1 : -1), 0);

            var subStates = new IUnitStateEngine[]
            {
                new LinearMoveToTargetState(graphics, graphicsRoot, targetPosition, animationSid),
                new DirectInteractionState(graphics, animationInfo, animationSid),
                new LinearMoveBackState(graphics, graphicsRoot, targetPosition, mainAnimationBlocker)
            };

            _innerState = new SequentialState(subStates);

            _blocker = mainAnimationBlocker;
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