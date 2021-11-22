using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class VampireBiteSkill : SkillBase
    {
        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.WolfBite
        };
        
        public VampireBiteSkill() : this(false)
        {
        }

        public VampireBiteSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new LifeDrawEffect
                    {
                        Actor = u,
                        DamageMultiplier = 1.0f
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Vampiric Bite";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;
    }
}