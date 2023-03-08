using System.Collections.Generic;

using Core.Dices;

using Microsoft.Xna.Framework;

using Rpg.Client.Assets.InteractionDeliveryObjects;
using Rpg.Client.Assets.States.Primitives;
using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat.GameObjects;
using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates;

namespace Rpg.Client.Assets.States.HeroSpecific
{
    internal class ArrowRainUsageState : IUnitStateEngine
    {
        private const int TOTAL_ARROW_COUNT = 10;
        private const float BASE_ARROW_LIFETIME_DURATION = 3f;
        private const float BASE_OFFSET_PERCENTAGE = 0.5f;
        private readonly IUnitStateEngine _mainContainerState;
        private readonly AnimationBlocker _mainStateBlocker;

        public ArrowRainUsageState(
            UnitGameObject animatedUnitGameObject,
            AnimationBlocker mainStateBlocker,
            ISkillVisualizationContext context)
        {
            var launchRainSourceState = CreateLaunchRainSourceState(
                context,
                animatedUnitGameObject.LaunchPoint,
                animatedUnitGameObject.Graphics);

            var arrowFallState = CreateArrowFallState(context);

            var subStates = new[]
            {
                launchRainSourceState,
                arrowFallState
            };

            _mainContainerState = new SequentialState(subStates);
            _mainStateBlocker = mainStateBlocker;
        }

        private static IUnitStateEngine CreateArrowFallState(ISkillVisualizationContext context)
        {
            var resultArrowsStates = new List<IUnitStateEngine>();

            var targetArea = context.BattlefieldInteractionContext.GetArea(Team.Cpu);

            // In the skill rules only 1 interaction.
            var materializedTargets = context.Interaction.SkillRuleInteractions[0].Targets;

            foreach (var materializedTarget in materializedTargets)
            {
                var materializedTargetGameObject = context.GetGameObject(materializedTarget);
                var materializedTargetGameObjectPosition = materializedTargetGameObject.InteractionPoint;

                var startPosition = CreateArrowOffset(materializedTargetGameObjectPosition);
                var targetPosition = materializedTargetGameObjectPosition;

                var arrowBlocker = context.AddAnimationBlocker();

                var arrowLifetimeRandomOffset = context.Dice.RollD100() / 100f * BASE_OFFSET_PERCENTAGE;
                var arrowDuration =
                    (BASE_ARROW_LIFETIME_DURATION - (BASE_ARROW_LIFETIME_DURATION * BASE_OFFSET_PERCENTAGE * 0.5f)) *
                    arrowLifetimeRandomOffset;

                void singleTargetAction(ICombatUnit combatUnit)
                {
                    context.Interaction.SkillRuleInteractions[0].Action(combatUnit);
                }

                var arrow = new EnergoArrowProjectile(startPosition, targetPosition,
                    context.GameObjectContentStorage, arrowBlocker,
                    lifetimeDuration: arrowDuration,
                    interaction: singleTargetAction,
                    targetCombatUnit: materializedTarget);

                var blast = new EnergyArrowBlast(
                    targetPosition,
                    context.GameObjectContentStorage.GetBulletGraphics(),
                    context.GameObjectContentStorage.GetParticlesTexture());

                var sequentialEffect = new SequentialProjectile(new IInteractionDelivery[] { arrow, blast });

                var arrowState =
                    new WaitInteractionDeliveryState(new[] { sequentialEffect }, context.InteractionDeliveryManager);
                resultArrowsStates.Add(arrowState);
            }

            for (var i = 0; i < TOTAL_ARROW_COUNT; i++)
            {
                var targetRandomPosition = context.Dice.RollPoint(targetArea);
                var startPosition = CreateArrowOffset(targetRandomPosition);

                var arrowBlocker = context.AddAnimationBlocker();

                var arrowLifetimeRandomOffset = context.Dice.RollD100() / 100f * BASE_OFFSET_PERCENTAGE;
                var arrowDuration =
                    (BASE_ARROW_LIFETIME_DURATION - (BASE_ARROW_LIFETIME_DURATION * BASE_OFFSET_PERCENTAGE * 0.5f)) *
                    arrowLifetimeRandomOffset;

                var arrow = new EnergoArrowProjectile(startPosition, targetRandomPosition,
                    context.GameObjectContentStorage, arrowBlocker, lifetimeDuration: arrowDuration);

                var blast = new EnergyArrowBlast(
                    targetRandomPosition,
                    context.GameObjectContentStorage.GetBulletGraphics(),
                    context.GameObjectContentStorage.GetParticlesTexture());

                var sequentialEffect = new SequentialProjectile(new IInteractionDelivery[] { arrow, blast });

                var arrowState =
                    new WaitInteractionDeliveryState(new[] { sequentialEffect }, context.InteractionDeliveryManager);
                resultArrowsStates.Add(arrowState);
            }

            return new ParallelState(resultArrowsStates);
        }

        private static Vector2 CreateArrowOffset(Vector2 targetPosition)
        {
            return targetPosition - Vector2.UnitY * 200 - Vector2.UnitX * 100;
        }


        private static IUnitStateEngine CreateLaunchRainSourceState(ISkillVisualizationContext context,
            Vector2 launchPoint, UnitGraphics animatedObjectGraphics)
        {
            var projectileBlocker = context.AddAnimationBlocker();
            var projectile = new EnergoArrowRainSourceProjectile(launchPoint,
                context.GameObjectContentStorage, projectileBlocker, lifetimeDuration: 2);

            var launchInteractionDeliveryState = new LaunchInteractionDeliveryState(
                animatedObjectGraphics,
                new[] { projectile },
                context.InteractionDeliveryManager,
                context.GetSoundEffect(GameObjectSoundType.EnergoShot),
                PredefinedAnimationSid.Skill3);

            return launchInteractionDeliveryState;
        }

        public bool CanBeReplaced => false;

        public bool IsComplete => _mainContainerState.IsComplete;

        public void Cancel()
        {
            _mainContainerState.Cancel();
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            _mainContainerState.Update(gameTime);

            if (_mainContainerState.IsComplete)
            {
                _mainStateBlocker.Release();
            }
        }
    }
}