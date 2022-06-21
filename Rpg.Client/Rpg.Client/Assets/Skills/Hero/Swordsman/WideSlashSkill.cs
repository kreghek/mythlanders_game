using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Swordsman
{
    internal class WideSlashSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.WideSwordSlash;

        public WideSlashSkill() : this(false)
        {
        }

        public WideSlashSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateDamage(SID, SkillDirection.AllLineEnemies, 0.5f)
        };

        public override SkillSid Sid => SkillSid.WideSwordSlash;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            AnimationSid = PredefinedAnimationSid.Skill2,
            Type = SkillVisualizationStateType.MassMelee,
            SoundEffectType = GameObjectSoundType.SwordSlash,
            IconOneBasedIndex = 2
        };
    }
}