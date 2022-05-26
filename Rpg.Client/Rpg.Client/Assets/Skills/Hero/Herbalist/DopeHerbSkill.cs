using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Herbalist
{
    internal class DopeHerbSkill : VisualizedSkillBase
    {
        public DopeHerbSkill() : this(false)
        {
        }

        public DopeHerbSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new StunEffect(u, new DurationEffectLifetime(new EffectDuration(3)));

                    return effect;
                })
            }
        };

        public override SkillSid Sid => SkillSid.DopeHerb;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.MagicDust,
            IconOneBasedIndex = 11
        };
    }
}