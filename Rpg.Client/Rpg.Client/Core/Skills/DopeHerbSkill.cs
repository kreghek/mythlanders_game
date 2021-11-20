using System;
using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class DopeHerbSkill : SkillBase
    {
        public DopeHerbSkill() : this(false)
        {
        }

        public DopeHerbSkill(bool costRequired) : base(new SkillVisualization
        { Type = SkillVisualizationStateType.Support, SoundEffectType = GameObjectSoundType.MagicDust },
            costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new StunEffect
                    {
                        Duration = 1
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