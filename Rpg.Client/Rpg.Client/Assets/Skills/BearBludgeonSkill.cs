using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
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
                EffectCreator = new EffectCreator((u, env) =>
                {
                    var res = new DamageEffect
                    {
                        Actor = u,
                        DamageMultiplier = 1.0f,
                        Env = env,
                    };

                    return res;
                })
            },
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator((u, env) =>
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