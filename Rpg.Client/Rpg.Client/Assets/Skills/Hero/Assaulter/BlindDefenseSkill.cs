using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets.InteractionDeliveryObjects;
using Rpg.Client.Assets.States.HeroSpecific;
using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Hero.Assaulter
{
    internal class BlindDefenseSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.BlindDefense;

        public BlindDefenseSkill() : this(false)
        {
        }

        public BlindDefenseSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = CreateRules();

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.Gunshot,
            IconOneBasedIndex = 3
        };

        public override IUnitStateEngine CreateState(UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainStateBlocker, ISkillVisualizationContext context)
        {
            var mainShotingBlocker = context.AddAnimationBlocker();
            var interactionItems = context.Interaction.SkillRuleInteractions
                .Where(x => (x.Metadata is AssaultSkillRuleMetadata meta) && meta.IsShot).ToArray();
            var bulletDataList = new List<(AnimationBlocker, IInteractionDelivery)>();
            for (var i = 0; i < interactionItems.Length; i++)
            {
                var item = interactionItems[i];
                var bulletAnimationBlocker = context.AddAnimationBlocker();

                var materializedTarget = interactionItems[i].Targets[0];
                var materializedTargetGameObject = context.GetGameObject(materializedTarget);
                var materializedTargetGameObjectPosition = materializedTargetGameObject.InteractionPoint;

                var singleInteractionDelivery = new EnergoArrowProjectile(animatedUnitGameObject.LaunchPoint,
                    materializedTargetGameObjectPosition,
                    context.GameObjectContentStorage,
                    bulletAnimationBlocker,
                    materializedTarget,
                    item.Action);

                bulletDataList.Add(new(bulletAnimationBlocker, singleInteractionDelivery));

                bulletAnimationBlocker.Released += (_, _) =>
                {
                    var allBuletBlockerIsReleased = !bulletDataList.Any(x => !x.Item1.IsReleased);
                    if (allBuletBlockerIsReleased)
                    {
                        mainShotingBlocker.Release();
                    }
                };
            }

            var animationBlocker = context.AnimationManager.CreateAndUseBlocker();

            StateHelper.HandleStateWithInteractionDelivery(
                context.Interaction.SkillRuleInteractions.First(x =>
                    (x.Metadata is AssaultSkillRuleMetadata meta) && meta.IsBuff),
                mainStateBlocker,
                mainShotingBlocker,
                animationBlocker);

            var state = new AssaultRifleBurstState(
                graphics: animatedUnitGameObject._graphics,
                animationBlocker,
                bulletDataList.Select(x => x.Item2).ToList(),
                interactionDeliveryManager: context.InteractionDeliveryList,
                rifleShotSound: context.GetHitSound(GameObjectSoundType.AssaultRifleBurst),
                animationSid: AnimationSid.Skill1);

            return state;
        }

        private static List<EffectRule> CreateRules()
        {
            var list = new List<EffectRule>();

            var buffEffect = SkillRuleFactory.CreateProtection(SID, SkillDirection.Self, multiplier: 1f);
            buffEffect.EffectMetadata = new AssaultSkillRuleMetadata
            {
                IsBuff = true
            };
            list.Add(buffEffect);

            for (var i = 0; i < 5; i++)
            {
                var rule = SkillRuleFactory.CreateDamage(SID, SkillDirection.RandomEnemy, multiplier: 0.1f);
                rule.EffectMetadata = new AssaultSkillRuleMetadata
                {
                    IsShot = true
                };
                list.Add(rule);
            }

            return list;
        }
    }
}