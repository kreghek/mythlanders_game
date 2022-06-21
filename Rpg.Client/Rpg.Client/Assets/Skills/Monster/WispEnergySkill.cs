using System.Collections.Generic;

using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Monster
{
    internal sealed class WispEnergySkill : VisualizedSkillBase
    {
        public WispEnergySkill() : base(PredefinedVisualization)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            SkillRuleFactory.CreateDamage(SkillSid.None, SkillDirection.AllLineEnemies, multiplier: 0.5f)
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