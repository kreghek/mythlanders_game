using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using Rpg.Client.Core.Effects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal sealed class PowerUpSkill : SkillBase
    {
        public PowerUpSkill() : this(false)
        {
        }

        public PowerUpSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new IncreaseAttackEffect(u.Unit.Support)
                    {
                        Duration = 3
                    };

                    return effect;
                })
            }
        };

        public override string Sid => "Power Up";
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Support,
            SoundEffectType = GameObjectSoundType.MagicDust
        };
    }
}