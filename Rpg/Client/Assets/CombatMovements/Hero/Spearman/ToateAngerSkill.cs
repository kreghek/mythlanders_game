using System.Collections.Generic;

using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills.Hero.Spearman
{
    internal class ToateAngerSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.EnergyShot;

        public ToateAngerSkill() : base(PredefinedVisualization, false)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateDamage(SID)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.EnergoShot
        };
    }
}