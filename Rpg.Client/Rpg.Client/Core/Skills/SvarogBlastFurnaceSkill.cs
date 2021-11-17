using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Models;

namespace Rpg.Client.Core.Skills
{
    internal class SvarogBlastFurnaceSkill : SkillBase
    {
        public SvarogBlastFurnaceSkill() : this(false)
        {
        }

        public SvarogBlastFurnaceSkill(bool costRequired) : base(new SkillVisualization
                { Type = SkillVisualizationStateType.MassRange, SoundEffectType = GameObjectSoundType.SwordSlash },
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
                        DamageMultiplier = 1.5f,
                        Actor = u
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Svarog's Blast Furnace";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;
    }
}