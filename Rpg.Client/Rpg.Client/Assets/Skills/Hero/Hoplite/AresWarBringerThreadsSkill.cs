using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Hoplite
{
    internal class AresWarBringerThreadsSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.AresWarBringerThreads;

        public AresWarBringerThreadsSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreatePowerUp(SID, SkillDirection.AllFriendly, duration: 3),
            new EffectRule
            {
                Direction = SkillDirection.AllFriendly,
                EffectCreator = new EffectCreator(u =>
                {
                    return new ResolveModifyEffect(u, new DurationEffectLifetime(new EffectDuration(3)), modifier: 1f);
                })
            }
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.MassRange,
            SoundEffectType = GameObjectSoundType.FireDamage,
            IconOneBasedIndex = 25
        };
    }
}