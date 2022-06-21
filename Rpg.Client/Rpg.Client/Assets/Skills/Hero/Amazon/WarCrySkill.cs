using System.Collections.Generic;

using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Amazon
{
    internal sealed class WarCrySkill : VisualizedSkillBase
    {
        public WarCrySkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreatePowerUp(SkillSid.WarCry, SkillDirection.AllFriendly, 3)
        };

        public override SkillSid Sid => SkillSid.WarCry;
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.AmazonWarCry,
            IconOneBasedIndex = 28,
        };
    }
}