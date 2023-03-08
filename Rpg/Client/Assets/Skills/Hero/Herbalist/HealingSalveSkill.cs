using System.Collections.Generic;

using Client.Core.SkillEffects;

using Rpg.Client.Assets.States.HeroSpecific;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Hero.Herbalist
{
    internal class HealingSalveSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.HealingSalve;

        public HealingSalveSkill() : base(PredefinedVisualization, costRequired: false)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new HealEffect(u)
                    {
                        PowerMultiplier = 0.6f
                    };

                    return effect;
                })
            },
            SkillRuleFactory.CreatePeriodicHealing(SID, 0.3f, 2, SkillDirection.Target)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.Heal,
            IconOneBasedIndex = 9
        };

        public override IUnitStateEngine CreateState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainAnimationBlocker,
            ISkillVisualizationContext context)
        {
            var state = new HerbalistHealingSalveUsageState(animatedUnitGameObject.Graphics, targetUnitGameObject,
                mainAnimationBlocker, context.Interaction, context.GetSoundEffect(GameObjectSoundType.Heal),
                context.GameObjectContentStorage, context.AnimationManager, context.InteractionDeliveryManager);

            return state;
        }
    }
}