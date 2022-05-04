using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills
{
    internal class DemonicTauntSkill : VisualizedSkillBase
    {
        public DemonicTauntSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                //TODO Make taunt effect
                Direction = SkillDirection.Self,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new DecreaseDamageEffect(u, 1, multiplier: 0.5f);

                    return effect;
                })
            }
        };

        public override SkillSid Sid => SkillSid.DemonicTaunt;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.None;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.Defence
        };
    }
}