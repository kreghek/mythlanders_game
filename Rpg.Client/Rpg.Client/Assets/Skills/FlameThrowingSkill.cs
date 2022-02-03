using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class FlameThrowingSkill : SkillBase
    {
        private const SkillSid SID = SkillSid.FlameThrowing;

        public FlameThrowingSkill() : this(false)
        {
        }

        public FlameThrowingSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemy,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplier = u.Unit.GetEquipmentAttackMultiplier(SID);
                    var res = new AttackEffect
                    {
                        Actor = u,
                        DamageMultiplier = 0.3f * equipmentMultiplier
                    };

                    return res;
                })
            },

            new EffectRule
            {
                Direction = SkillDirection.AllEnemy,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplier = u.Unit.GetEquipmentAttackMultiplier(SID);
                    var res = new PeriodicDamageEffect
                    {
                        Actor = u,
                        PowerMultiplier = 0.2f * equipmentMultiplier,
                        Duration = 3
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