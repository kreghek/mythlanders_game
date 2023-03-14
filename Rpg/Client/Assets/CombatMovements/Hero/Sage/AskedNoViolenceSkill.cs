using System.Collections.Generic;

using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills.Hero.Sage
{
    internal class AskedNoViolenceSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.AskedNoViolence;

        public AskedNoViolenceSkill() : base(PredefinedVisualization, false)
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