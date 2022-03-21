using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class MassHealSkill : SkillBase
    {
        public MassHealSkill() : this(false)
        {
        }

        public MassHealSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllFriendly,
                EffectCreator = new EffectCreator((u, env) =>
                {
                    var effect = new HealEffect
                    {
                        Actor = u,
                        PowerMultiplier = 0.7f
                    };

                    return effect;
                })
            }
        };

        public override SkillSid Sid => SkillSid.MassHeal;
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Support,
            SoundEffectType = GameObjectSoundType.Heal
        };
    }
}