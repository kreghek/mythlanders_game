using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class VampiricBiteSkill : SkillBase
    {
        public VampiricBiteSkill() : this(false)
        {
        }

        public VampiricBiteSkill(bool costRequired) : base(new SkillVisualization
        { Type = SkillVisualizationStateType.Melee, SoundEffectType = GameObjectSoundType.WolfBite }, costRequired)
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
                        PowerMultiplier = 0.5f,
                        ValueRange = 1
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

        public override string Sid => "Vampiric Bite";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;
    }
}