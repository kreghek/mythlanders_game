using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class ReproachSkill : SkillBase
    {
        private const SkillSid SID = SkillSid.Reproach;

        public ReproachSkill() : this(false)
        {
        }

        public ReproachSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemies,
                EffectCreator = new EffectCreator((u, env) =>
                {
                    var equipmentMultiplier = u.Unit.GetEquipmentAttackMultiplier(SID);

                    var res = new PeriodicDamageEffect
                    {
                        PowerMultiplier = 0.5f * equipmentMultiplier,
                        Actor = u
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SkillSid.WideSwordSlash;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.MassMelee,
            SoundEffectType = GameObjectSoundType.SwordSlash
        };
    }
}