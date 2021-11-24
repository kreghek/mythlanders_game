using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class ArrowRainSkill : SkillBase
    {
        public ArrowRainSkill() : this(false)
        {
        }

        public ArrowRainSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
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

        public override string Sid => "Arrow Rain";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.MassRange,
            SoundEffectType = GameObjectSoundType.BowShot
        };
    }
}