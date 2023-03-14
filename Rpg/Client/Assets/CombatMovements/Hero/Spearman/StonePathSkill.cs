using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills.Hero.Spearman
{
    internal class StonePathSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.StonePath;

        public StonePathSkill() : this(false)
        {
        }

        public StonePathSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateProtection(SID, 0.5f)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.None;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.Defence,
            AnimationSid = PredefinedAnimationSid.Skill3
        };
    }
}