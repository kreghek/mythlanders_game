using System.Collections.Generic;

using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills.Monster
{
    internal sealed class PowerUpSkill : VisualizedSkillBase
    {
        public PowerUpSkill() : this(false)
        {
        }

        public PowerUpSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreatePowerUp(SkillSid.None, SkillDirection.Target, 3)
        };

        public override SkillSid Sid => SkillSid.PowerUp;
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.MagicDust
        };
    }
}