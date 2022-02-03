using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class ToxicHerbsSkill : SkillBase
    {
        public ToxicHerbsSkill() : base(PredefinedVisualization, costRequired: false)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
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
            },
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new AttackEffect
                    {
                        Actor = u,
                        DamageMultiplier = 0.3f
                    };

                    return effect;
                })
            }
        };

        public override SkillSid Sid => SkillSid.ToxicHerbs;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.EnergoShot
        };
    }
}