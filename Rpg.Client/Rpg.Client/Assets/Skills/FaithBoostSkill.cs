using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal sealed class FaithBoostSkill : SkillBase
    {
        public FaithBoostSkill() : this(false)
        {
        }

        public FaithBoostSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
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

        public override SkillSid Sid => SkillSid.FaithBoost;
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Support,
            SoundEffectType = GameObjectSoundType.MagicDust
        };
    }
}