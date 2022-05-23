using System.Collections.Generic;

using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Archer
{
    internal class ZduhachMightSkill : VisualizedSkillBase
    {
        public ZduhachMightSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateProtection(SkillSid.ZduhachMight, SkillDirection.Self, 1, 1)
        };

        public override SkillSid Sid => SkillSid.ZduhachMight;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.None;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.Defence,
            IconOneBasedIndex = 8
        };
    }
}