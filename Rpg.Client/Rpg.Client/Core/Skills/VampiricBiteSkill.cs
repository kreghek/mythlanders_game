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
                    var res = new LifeDrawEffect
                    {
                        Actor = u,
                        DamageMultiplier = 1.0f
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