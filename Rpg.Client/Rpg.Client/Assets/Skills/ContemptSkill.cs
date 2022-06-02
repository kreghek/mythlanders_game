using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills
{
    internal class ContemptSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.JavelinThrow;

        public ContemptSkill() : this(false)
        {
        }

        public ContemptSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateDamage(SID, SkillDirection.Target, multiplier: 0.5f),
            SkillRuleFactory.CreateDamage(SID, SkillDirection.Target, multiplier: 0.5f),
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(_=> new PushBackEffect())
            }
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.EnergoShot
        };
    }
}