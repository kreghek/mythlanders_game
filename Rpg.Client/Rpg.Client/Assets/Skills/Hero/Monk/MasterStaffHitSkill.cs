using System.Collections.Generic;

using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Monk
{
    internal class MasterStaffHitSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.MasterStaffHit;

        private static readonly SkillVisualization _predefinedSkillVisualization = new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.StaffHit,
            IconOneBasedIndex = 17
        };

        public MasterStaffHitSkill(bool costRequired) : base(_predefinedSkillVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            SkillRuleFactory.CreateDamage(SID)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;
    }
}