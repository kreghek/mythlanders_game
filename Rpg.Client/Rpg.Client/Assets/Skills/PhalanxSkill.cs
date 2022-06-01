using System.Collections.Generic;

using Rpg.Client.Assets.TargetSelectors;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills
{
    internal class PhalanxSkill : VisualizedSkillBase
    {
        public PhalanxSkill() : this(false)
        {
        }

        private PhalanxSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateProtection(SkillSid.DefenseStance, SkillDirection.Self, 1, 0.75f),
            SkillRuleFactory.CreateProtection(SkillSid.DefenseStance, new LeftAllyTargetSelector(), duration: 1, multiplier: 0.75f),
            SkillRuleFactory.CreateProtection(SkillSid.DefenseStance, new LeftAllyTargetSelector(), duration: 1, multiplier: 0.25f, imposeCondition:IsRightWithShield)
        };

        private static bool IsRightWithShield(CombatEffectContext combatSkillContext)
        {
            return false;
        }

        public override SkillSid Sid => SkillSid.DefenseStance;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.None;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.Defence
        };
    }
}