using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Rpg.Client.Assets.InteractionDeliveryObjects;
using Rpg.Client.Assets.States;
using Rpg.Client.Assets.States.HeroSpecific;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
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

        private static List<EffectRule> CreateRules()
        {
            var list = new List<EffectRule>
            {
                SkillRuleFactory.CreateProtection(SID, multiplier: 1f, SkillDirection.Self)
            };

            list.Add(new EffectRule
            {
                Direction = SkillDirection.RandomEnemy,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplier = u.Unit.GetEquipmentDamageMultiplierBonus(SID);
                    var res = new DamageEffect
                    {
                        Actor = u,
                        DamageMultiplier = 0.01f * equipmentMultiplier
                    };

                    return res;
                })
            });

            return list;
        }

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.Gunshot,
            IconOneBasedIndex = 3
        };

        public override IUnitStateEngine CreateState(UnitGameObject animatedUnitGameObject, UnitGameObject targetUnitGameObject,
            AnimationBlocker mainStateBlocker, ISkillVisualizationContext context)
        {
            var interactionDeliveryBlocker = context.AnimationManager.CreateAndUseBlocker();

            var materializedTarget = context.Interaction.SkillRuleInteractions[1].Targets[0];
            var materializedTargetGameObject = context.GetGameObject(materializedTarget).Position - Vector2.UnitY * (64);
            
            var singleInteractionDelivery = new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64),
                materializedTargetGameObject,
                context.GameObjectContentStorage,
                interactionDeliveryBlocker);
            
            var animationBlocker = context.AnimationManager.CreateAndUseBlocker();

            StateHelper.HandleStateWithInteractionDelivery(context.Interaction.SkillRuleInteractions,
                mainStateBlocker, 
                interactionDeliveryBlocker,
                animationBlocker);

            var state = new CommonDistantSkillUsageState(
                graphics: animatedUnitGameObject._graphics,
                animationBlocker,
                interactionDelivery: new[] { singleInteractionDelivery },
                interactionDeliveryList: context.InteractionDeliveryList,
                hitSound: context.GetHitSound(GameObjectSoundType.Gunshot),
                animationSid: AnimationSid.Skill1);

            return state;
        }
    }
}