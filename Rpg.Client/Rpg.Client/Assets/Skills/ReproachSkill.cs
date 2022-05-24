using System.Collections.Generic;

using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills
{
    internal class ReproachSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.Reproach;

        public ReproachSkill() : this(false)
        {
        }

        public ReproachSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreatePeriodicDamage(SkillSid.None, 1, SkillDirection.AllEnemies)
        };

        public override SkillSid Sid => SkillSid.WideSwordSlash;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.MassMelee,
            SoundEffectType = GameObjectSoundType.SwordSlash
        };
    }
}