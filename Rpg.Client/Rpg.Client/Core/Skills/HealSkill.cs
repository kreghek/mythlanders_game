using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class HealSkill : SkillBase
    {
        public HealSkill() : this(false)
        {
        }

        public HealSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new HealEffect
                    {
                        Actor = u,
                        PowerMultiplier = 1
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Heal";
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Telekinetic;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Support,
            SoundEffectType = GameObjectSoundType.Heal
        };
    }
}