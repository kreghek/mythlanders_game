using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Swordsman
{
    internal class SwordSlashSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.SwordSlash;

        public SwordSlashSkill() : this(false)
        {
        }

        private SwordSlashSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            SkillRuleFactory.CreateDamage(SID)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            AnimationSid = AnimationSid.Skill1,
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.SwordSlash,
            IconOneBasedIndex = 1
        };
    }
}