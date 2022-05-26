using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Archer
{
    internal class RapidShotSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.RapidShot;

        public RapidShotSkill() : this(false)
        {
        }

        public RapidShotSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateDamage(SID, SkillDirection.Target, multiplier: 1f, scatter: 0.5f),
            SkillRuleFactory.CreatePowerUp(SID, SkillDirection.Self)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.EnergoShot,
            IconOneBasedIndex = 6,
            AnimationSid = PredefinedAnimationSid.Skill1
        };
    }
}