using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class WideSlashSkill : SkillBase
    {
        public WideSlashSkill() : this(false)
        {
        }

        public WideSlashSkill(bool costRequired) : base(new SkillVisualization
        { Type = SkillVisualizationStateType.MassMelee, SoundEffectType = GameObjectSoundType.SwordSlash },
            costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemy,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new AttackEffect
                    {
                        PowerMultiplier = 0.5f,
                        ValueRange = 1,
                        Actor = u
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Wide Slash";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;
    }

    internal class VolkolakClawsSkill : SkillBase
    {
        public VolkolakClawsSkill() : this(false)
        {
        }

        public VolkolakClawsSkill(bool costRequired) : base(new SkillVisualization
        { Type = SkillVisualizationStateType.MassMelee, SoundEffectType = GameObjectSoundType.WolfBite },
            costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemy,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new AttackEffect
                    {
                        PowerMultiplier = 0.5f,
                        ValueRange = 1,
                        Actor = u
                    };

                    return res;
                })
            },
            new EffectRule
            {
                Direction = SkillDirection.Self,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new HealEffect
                    {
                        Actor = u,
                        PowerMultiplier = 0.5f,
                        ValueRange = 1
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Wide Slash";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;
    }
}