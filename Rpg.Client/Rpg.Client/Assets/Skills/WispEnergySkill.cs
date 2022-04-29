using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills
{
    internal sealed class WispEnergySkill : VisualizedSkillBase
    {
        public WispEnergySkill() : base(PredefinedVisualization)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemies,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new DamageEffect
                    {
                        DamageMultiplier = 0.5f,
                        Actor = u
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SkillSid.WispEnergy;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.MassRange,
            SoundEffectType = GameObjectSoundType.WispEnergy
        };
    }
}