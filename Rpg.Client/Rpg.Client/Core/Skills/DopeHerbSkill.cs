using System;
using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class DopeHerbSkill : SkillBase
    {
        public DopeHerbSkill()
        {
        }

        public DopeHerbSkill(bool costRequired) : base(costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var rawEffectValue = (double)u.Unit.Level / 5;

                    var effect = new DopeHerbEffect
                    {
                        Value = (int)Math.Ceiling(rawEffectValue)
                    };

                    return effect;
                })
            }
        };

        public override string Sid => "Dope Herb";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;
    }
}