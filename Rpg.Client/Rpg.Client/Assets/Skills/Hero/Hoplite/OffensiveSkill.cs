using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Hoplite
{
    internal class OffensiveSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.Offensive;

        public OffensiveSkill() : this(false)
        {
        }

        private OffensiveSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            SkillRuleFactory.CreateDamage(SID),
            SkillRuleFactory.CreatePeriodicDamage(SID, duration: 3, SkillDirection.Target)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.SwordSlash,
            IconOneBasedIndex = 22,
            AnimationSid = PredefinedAnimationSid.Skill1
        };
    }
}