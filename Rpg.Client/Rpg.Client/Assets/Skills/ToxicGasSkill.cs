using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class ToxicGasSkill : SkillBase
    {
        public ToxicGasSkill() : base(PredefinedVisualization, costRequired: false)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator((u, env) =>
                {
                    var effect = new PeriodicSupportAttackEffect
                    {
                        Actor = u,
                        SourceSupport = u.Unit.Support,
                        PowerMultiplier = 0.3f,
                        Duration = 3
                    };

                    return effect;
                })
            }
        };

        public override SkillSid Sid => SkillSid.ToxicGas;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.EnergoShot
        };
    }
}