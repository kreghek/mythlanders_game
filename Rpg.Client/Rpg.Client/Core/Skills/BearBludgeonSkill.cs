using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class BearBludgeonSkill : SkillBase
    {
        public BearBludgeonSkill() : base(PredefinedVisualization, false)
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
                        DamageMultiplier = 1.0f
                    };

                    return res;
                })
            },
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new StunEffect
                    {
                        Duration = 1
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SkillSid.MutantBearBludgeon;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.BearBludgeon
        };
    }
}