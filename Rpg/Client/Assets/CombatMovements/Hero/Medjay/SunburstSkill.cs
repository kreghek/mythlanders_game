using System.Collections.Generic;

using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills.Hero.Medjay
{
    internal class SunburstSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.Sunburst;

        public SunburstSkill() : base(PredefinedVisualization, false)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreatePowerDown(SID, SkillDirection.AllEnemies, duration: 3)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.MassRange,
            SoundEffectType = GameObjectSoundType.FireDamage,
            IconOneBasedIndex = 32
        };
    }
}