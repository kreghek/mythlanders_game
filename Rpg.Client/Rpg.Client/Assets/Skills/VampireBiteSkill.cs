using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class VampireBiteSkill : SkillBase
    {
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

        public override SkillSid Sid => SkillSid.VampireBite;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.VampireBite
        };
    }
}