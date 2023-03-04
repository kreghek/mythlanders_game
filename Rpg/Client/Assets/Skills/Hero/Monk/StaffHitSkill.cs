using System.Collections.Generic;

using Client.GameScreens.Combat.GameObjects;

using Rpg.Client.Assets.States.HeroSpecific;
using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Hero.Monk
{
    internal class StaffHitSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.StaffHit;

        private static readonly SkillVisualization _predefinedSkillVisualization = new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.StaffHit,
            AnimationSid = PredefinedAnimationSid.Skill1,
            IconOneBasedIndex = 12
        };

        public StaffHitSkill() : this(false)
        {
        }

        private StaffHitSkill(bool costRequired) : base(_predefinedSkillVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            SkillRuleFactory.CreateDamage(SID, SkillDirection.Target, 0.25f),
            SkillRuleFactory.CreateDamage(SID, SkillDirection.Target, 0.25f),
            SkillRuleFactory.CreateDamage(SID, SkillDirection.Target, 0.50f)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        public override IActorVisualizationState CreateState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainAnimationBlocker,
            ISkillVisualizationContext context)
        {
            var state = new MonkTripleHitState(animatedUnitGameObject.Graphics, targetUnitGameObject.Graphics,
                mainAnimationBlocker, context.Interaction, new[]
                {
                    context.GetSoundEffect(GameObjectSoundType.StaffHit),
                    context.GetSoundEffect(GameObjectSoundType.StaffHit),
                    context.GetSoundEffect(GameObjectSoundType.StaffHit)
                });
            return state;
        }
    }
}