using System.Collections.Generic;

using Rpg.Client.Assets.States.HeroSpecific;
using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Hero.Herbalist
{
    internal class ToxicGasSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.ToxicGas;

        public ToxicGasSkill() : base(PredefinedVisualization, costRequired: false)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            SkillRuleFactory.CreatePeriodicDamage(SID, 2, SkillDirection.Target)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.EnergoShot,
            IconOneBasedIndex = 10
        };

        public override IUnitStateEngine CreateState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainAnimationBlocker,
            ISkillVisualizationContext context)
        {
            var state = new HerbalistToxicGasUsageState(animatedUnitGameObject.Graphics, targetUnitGameObject,
                mainAnimationBlocker, context.Interaction, context.GetSoundEffect(GameObjectSoundType.Heal),
                context.GameObjectContentStorage, context.AnimationManager, context.InteractionDeliveryManager);

            return state;
        }
    }
}