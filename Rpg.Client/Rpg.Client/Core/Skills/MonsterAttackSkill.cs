﻿using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class MonsterAttackSkill : SkillBase
    {
        public override string Sid => "Monster Attack";
        public override SkillType Type => SkillType.Melee;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new AttackEffect
                    {
                        Power = u.Unit.Power,
                        PowerMultiplier = 1,
                        ValueRange = 1,
                    };

                    return res;
                })
            }
        };
    }
}