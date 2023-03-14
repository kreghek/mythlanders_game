using System.Collections.Generic;

using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills.Hero.Priest
{
    internal class FingerOfAnubisShotSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.FingerOfAnubis;

        public FingerOfAnubisShotSkill() : base(PredefinedVisualization, false)
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
            SoundEffectType = GameObjectSoundType.EnergoShot,
            IconOneBasedIndex = 37
        };
    }
}