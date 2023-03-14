using System.Collections.Generic;

using JetBrains.Annotations;

using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills.Hero.Amazon
{
    [UsedImplicitly]
    internal class TribeDefenderSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.TribeDefender;

        public TribeDefenderSkill() : base(PredefinedVisualization, false)
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
            IconOneBasedIndex = 29
        };
    }
}