using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class WideSlashSkill : SkillBase
    {
        public WideSlashSkill() : this(false)
        {
        }

        public WideSlashSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
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
                        DamageMultiplier = 0.5f,
                        Actor = u
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Wide Slash";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.MassMelee,
            SoundEffectType = GameObjectSoundType.SwordSlash
        };
    }
}