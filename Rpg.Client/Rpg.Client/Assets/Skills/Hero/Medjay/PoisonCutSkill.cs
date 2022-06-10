using System.Collections.Generic;

using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Medjay
{
    internal class PoisonCutSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.PoisonedSpear;

        public PoisonCutSkill() : this(false)
        {
        }

        public PoisonCutSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            SkillRuleFactory.CreateDamage(SID),
            SkillRuleFactory.CreatePeriodicDamage(SID, 3, SkillDirection.Target)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.SwordSlash,
            IconOneBasedIndex = 30
        };
    }
}