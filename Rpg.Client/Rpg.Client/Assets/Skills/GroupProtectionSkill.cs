using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class GroupProtectionSkill : SkillBase
    {
        public GroupProtectionSkill() : this(false)
        {
        }

        public GroupProtectionSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllFriendly,
                EffectCreator = new EffectCreator((u, env) =>
                {
                    var effect = new DecreaseDamageEffect(multiplier: 0.5f) { Duration = 1 };

                    return effect;
                })
            }
        };

        public override SkillSid Sid => SkillSid.GroupProtection;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.None;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Support,
            SoundEffectType = GameObjectSoundType.Defence,
            AnimationSid = Core.AnimationSid.Skill3
        };
    }
}