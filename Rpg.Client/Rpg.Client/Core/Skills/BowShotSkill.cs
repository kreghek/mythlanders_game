using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class BowShotSkill : SkillBase
    {
        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.BowShot
        };

        public BowShotSkill() : this(false)
        {
        }

        public BowShotSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new AttackEffect
                    {
                        Actor = u,
                        DamageMultiplier = 1f
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Strike";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;
    }
}