using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Herbalist
{
    internal class MassHealSkill : VisualizedSkillBase
    {
        public MassHealSkill() : base(PredefinedVisualization, false)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllFriendly,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new HealEffect(u)
                    {
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
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.Heal,
            IconOneBasedIndex = 12
        };
    }
}