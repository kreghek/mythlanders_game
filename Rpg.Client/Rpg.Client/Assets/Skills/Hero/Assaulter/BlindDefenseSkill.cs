using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Assaulter
{
    internal class BlindDefenseSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.BlindDefense;

        public BlindDefenseSkill() : this(false)
        {
        }

        public BlindDefenseSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Self,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new DecreaseDamageEffect(multiplier: 1f)
                    {
                        Duration = 1
                    };

                    return effect;
                })
            },

            new EffectRule
            {
                Direction = SkillDirection.RandomEnemy,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplier = u.Unit.GetEquipmentAttackMultiplier(SID);
                    var res = new DamageEffect
                    {
                        Actor = u,
                        DamageMultiplier = 0.1f * equipmentMultiplier
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.Gunshot
        };
    }
}