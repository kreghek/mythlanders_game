using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills
{
    internal class FlameThrowingSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.FlameThrowing;

        public FlameThrowingSkill() : this(false)
        {
        }

        public FlameThrowingSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemies,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplier = u.Unit.GetEquipmentDamageMultiplierBonus(SID);
                    var res = new DamageEffect
                    {
                        Actor = u,
                        DamageMultiplier = 0.3f * equipmentMultiplier
                    };

                    return res;
                })
            },

            new EffectRule
            {
                Direction = SkillDirection.AllEnemies,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplier = u.Unit.GetEquipmentDamageMultiplierBonus(SID);
                    var res = new PeriodicDamageEffect(u, 3)
                    {
                        PowerMultiplier = 0.2f * equipmentMultiplier
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.EnergoShot
        };
    }
}