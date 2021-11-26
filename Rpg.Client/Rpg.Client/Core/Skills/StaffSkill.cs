using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class StaffSkill : SkillBase
    {
        public StaffSkill() : this(false)
        {
        }

        public StaffSkill(bool costRequired) : base(new SkillVisualization
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.StaffHit
        }, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new AttackEffect
                    {
                        Actor = u,
                        DamageMultiplier = 0.25f
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SkillSid.StaffHit;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        public override int UsageCount => 3;
    }
}