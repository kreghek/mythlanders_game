using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class SwordSlashSkill : SkillBase
    {
        public SwordSlashSkill() : this(false)
        {
        }

        public SwordSlashSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
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
                        DamageMultiplier = 1
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Slash";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        public override int UsageCount => 3;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.SwordSlash
        };
    }

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

        public override string Sid => "Staff hit";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        public override int UsageCount => 3;
    }
}