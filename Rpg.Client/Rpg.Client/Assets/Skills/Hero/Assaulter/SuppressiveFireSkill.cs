using System.Collections.Generic;

using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Assaulter
{
    internal class SuppressiveFireSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.SuppressiveFire;

        public SuppressiveFireSkill() : this(false)
        {
        }

        private SuppressiveFireSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = CreateRules();

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.Gunshot,
            IconOneBasedIndex = 16
        };

        private static List<EffectRule> CreateRules()
        {
            var list = new List<EffectRule>
            {
                SkillRuleFactory.CreatePowerDown(SID, SkillDirection.Target, 1)
            };

            for (var i = 0; i < 5; i++)
            {
                list.Add(SkillRuleFactory.CreateDamage(SID, SkillDirection.Target, 0.1f));
            }

            return list;
        }
    }
}