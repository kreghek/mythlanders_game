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
    internal class SuppressiveFireSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.SuppressiveFire;

        public SuppressiveFireSkill() : this(false)
        {
        }

        private SuppressiveFireSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = CreateRules();

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.Gunshot,
            IconOneBasedIndex = 16
        };

        private static List<EffectRule> CreateRules()
        {
            var list = new List<EffectRule>
            {
                SkillRuleFactory.CreatePowerDown(SID, SkillDirection.Target, 1)
            };

            for (var i = 0; i < 5; i++)
            {
                var rule = SkillRuleFactory.CreateDamage(SID, SkillDirection.Target, 0.1f);
                rule.EffectMetadata = new BlindDefenseSkillRuleMetadata() { IsShot = true };
                list.Add(rule);
            }

            return list;
        }

        public override IUnitStateEngine CreateState(UnitGameObject animatedUnitGameObject, UnitGameObject targetUnitGameObject,
            AnimationBlocker mainStateBlocker, ISkillVisualizationContext context)
        {
            var mainShotingBlocker = context.AddAnimationBlocker();
            var interactionItems = context.Interaction.SkillRuleInteractions.Where(x => (x.Metadata is BlindDefenseSkillRuleMetadata meta) && meta.IsShot).ToArray();
            var bulletDataList = new List<(AnimationBlocker, IInteractionDelivery)>();
            for (var i = 0; i < interactionItems.Length; i++)
            {
                var item = interactionItems[i];
                var bulletAnimationBlocker = context.AddAnimationBlocker();

                var materializedTarget = interactionItems[i].Targets[0];
                var materializedTargetGameObject = context.GetGameObject(materializedTarget);
                var materializedTargetGameObjectPosition = materializedTargetGameObject.InteractionPoint;

                var singleInteractionDelivery = new BulletGameObject(animatedUnitGameObject.LaunchPoint,
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

            mainShotingBlocker.Released += (_, _) =>
            {
                context.Interaction.SkillRuleInteractions[0].Action(context.Interaction.SkillRuleInteractions[0].Targets[0]);
            };

            var animationBlocker = context.AnimationManager.CreateAndUseBlocker();

            StateHelper.HandleStateWithInteractionDelivery(context.Interaction.SkillRuleInteractions,
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
    }
}