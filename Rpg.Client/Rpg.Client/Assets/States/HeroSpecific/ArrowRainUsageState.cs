using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Rpg.Client.Assets.InteractionDeliveryObjects;
using Rpg.Client.Assets.States.Primitives;
using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.HeroSpecific
{
    internal class ArrowRainUsageState : IUnitStateEngine
    {
        private readonly AnimationBlocker _mainStateBlocker;
        private readonly IUnitStateEngine _mainSequentialState;

        public ArrowRainUsageState(
            UnitGameObject animatedUnitGameObject,
            AnimationBlocker mainStateBlocker,
            ISkillVisualizationContext context)
        {
            var arrowRainSourceBlocker = context.AddAnimationBlocker();
            var arrowRainSource = new EnergoArrowRainSourceProjectile(animatedUnitGameObject.LaunchPoint,
                context.GameObjectContentStorage, arrowRainSourceBlocker, lifetimeDuration: 2);

            var mainDeliveryBlocker = context.AddAnimationBlocker();

            var fallingArrowList = new List<IInteractionDelivery>();

            for (var i = 0; i < 10; i++)
            {
                AnimationBlocker? blocker = null;
                if (i == 0)
                {
                    blocker = mainDeliveryBlocker;
                }

                var targetArea = context.BattlefieldInteractionContext.GetArea(Team.Cpu);
                var targetRandomPosition = context.Dice.RollPoint(targetArea);
                var startPosition = targetRandomPosition - Vector2.UnitY * 200;

                var arrow = new EnergoArrowProjectile(startPosition, targetRandomPosition,
                    context.GameObjectContentStorage, blocker, lifetimeDuration: 2);

                fallingArrowList.Add(arrow);
            }

            var stateAnimationBlocker = context.AddAnimationBlocker();

            StateHelper.HandleStateWithInteractionDelivery(context.Interaction.SkillRuleInteractions,
                mainStateBlocker,
                mainDeliveryBlocker,
                stateAnimationBlocker);

            var animatedObjectGraphics = animatedUnitGameObject.Graphics;

            var subStates = new IUnitStateEngine[]
            {
                new LaunchInteractionDeliveryState(
                    animatedObjectGraphics, 
                    new []{ arrowRainSource },
                    context.InteractionDeliveryManager,
                    context.GetHitSound(GameScreens.GameObjectSoundType.EnergoShot),
                    PredefinedAnimationSid.Skill3),

                new CreateImmeditlyInteractionDeliveryState(fallingArrowList, context.InteractionDeliveryManager)
            };
            _mainStateBlocker = mainStateBlocker;

            _mainSequentialState = new SequentialState(subStates);
        }

        public bool CanBeReplaced => false;

        public bool IsComplete => _mainSequentialState.IsComplete;

        public void Cancel()
        {
            _mainSequentialState.Cancel();
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            _mainSequentialState.Update(gameTime);

            if (_mainSequentialState.IsComplete)
            {
                _mainStateBlocker.Release();
            }
        }
    }
}
