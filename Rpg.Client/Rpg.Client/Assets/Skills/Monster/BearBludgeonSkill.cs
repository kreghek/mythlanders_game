using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Monster
{
    internal class BearBludgeonSkill : VisualizedSkillBase
    {
        public BearBludgeonSkill() : base(PredefinedVisualization, false)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            SkillRuleFactory.CreateDamage(SkillSid.None),
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

        public override SkillSid Sid => SkillSid.MutantBearBludgeon;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.BearBludgeon,
            AnimationSid = Core.PredefinedAnimationSid.Skill1
        };
    }
}