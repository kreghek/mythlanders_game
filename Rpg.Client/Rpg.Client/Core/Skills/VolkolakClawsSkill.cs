using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
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
                        DamageMultiplier = 1f,
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
                        PowerMultiplier = 0.3f
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Volkolak Claws";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;
    }
}