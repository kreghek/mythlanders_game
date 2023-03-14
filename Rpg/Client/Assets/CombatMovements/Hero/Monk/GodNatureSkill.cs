using System.Collections.Generic;

using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills.Hero.Monk
{
    internal class GodNatureSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.GodNature;

        public GodNatureSkill() : base(PredefinedVisualization, false)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            SkillRuleFactory.CreateDamage(SID),
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new StunEffect(u, new DurationEffectLifetime(new EffectDuration(1)));

                    return res;
                })
            }
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.BearBludgeon
        };
    }
}