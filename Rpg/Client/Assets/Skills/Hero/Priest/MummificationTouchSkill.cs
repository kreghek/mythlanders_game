using System.Collections.Generic;

using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Priest
{
    internal class UnlimitedSinSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.UnlimitedSin;

        public UnlimitedSinSkill() : this(false)
        {
        }

        private UnlimitedSinSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            SkillRuleFactory.CreateProtection(SID, 1f)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.Defence,
            IconOneBasedIndex = 35
        };
    }
}