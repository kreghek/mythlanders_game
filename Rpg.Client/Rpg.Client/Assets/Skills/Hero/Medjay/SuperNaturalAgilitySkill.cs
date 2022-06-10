using System.Collections.Generic;

using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Medjay
{
    internal class SuperNaturalAgilitySkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.SuperNaturalAgility;

        public SuperNaturalAgilitySkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateProtection(SID, SkillDirection.Self, duration: 1, multiplier: 0.5f)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.None;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.Defence,
            IconOneBasedIndex = 31
        };
    }
}