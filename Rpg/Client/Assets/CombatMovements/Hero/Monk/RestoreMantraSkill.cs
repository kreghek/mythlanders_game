using System.Collections.Generic;

using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills.Hero.Monk
{
    internal class RestoreMantraSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.RestoreMantra;

        public RestoreMantraSkill() : this(false)
        {
        }

        private RestoreMantraSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreatePeriodicHealing(SID, 0.3f, duration: 3, SkillDirection.AllFriendly)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.Heal
        };
    }
}