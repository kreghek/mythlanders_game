using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class VolkolakClawsSkill : SkillBase
    {
        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.MassMelee,
            SoundEffectType = GameObjectSoundType.WolfBite
        };

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
                Direction = SkillDirection.AllEnemy,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new LifeDrawEffect
                    {
                        DamageMultiplier = 0.3f,
                        Actor = u
                    };

                    return res;
                })
            }
        };

        public override string Sid => "Volkolak Claws";
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;
    }
}