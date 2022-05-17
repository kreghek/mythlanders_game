using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Sergeant
{
    internal class InspiringRushSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.InspiringRush;

        public InspiringRushSkill() : base(PredefinedVisualization, false)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            SkillRuleFactory.CreateDamage(SID, SkillDirection.Target, 0.5f),
            SkillRuleFactory.CreatePowerUp(SID, SkillDirection.RandomFriendly, 1)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            AnimationSid = PredefinedAnimationSid.Skill1,
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.SwordSlash,
            IconOneBasedIndex = 1
        };
    }
}