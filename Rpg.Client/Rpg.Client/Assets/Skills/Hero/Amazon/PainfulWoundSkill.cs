using System.Collections.Generic;

using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Amazon
{
    internal class PainfulWoundSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.PainfullWound;

        public PainfulWoundSkill() : this(false)
        {
        }

        public PainfulWoundSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateDamage(SID),
            SkillRuleFactory.CreatePeriodicDamage(SID, 3, SkillDirection.Target)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.SwordSlash,
            IconOneBasedIndex = 29
        };
    }
}