using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class VolkolakClawsSkill : SkillBase
    {
        public VolkolakClawsSkill() : this(false)
        {
        }

        public VolkolakClawsSkill(bool costRequired) : base(PredefinedVisualization,
            costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemies,
                EffectCreator = new EffectCreator((u, env) =>
                {
                    var res = new LifeDrawEffect
                    {
                        DamageMultiplier = 0.5f,
                        Actor = u
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SkillSid.VolkolakClaws;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.MassMelee,
            SoundEffectType = GameObjectSoundType.WolfBite
        };
    }
}