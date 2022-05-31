using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Archer
{
    internal class EnergyShotSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.EnergyShot;

        public EnergyShotSkill() : this(false)
        {
        }

        private EnergyShotSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateDamage(SID)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.EnergoShot,
            IconOneBasedIndex = 5,
            AnimationSid = PredefinedAnimationSid.Skill1
        };
    }
}