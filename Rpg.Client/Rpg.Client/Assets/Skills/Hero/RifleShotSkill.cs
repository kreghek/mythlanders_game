using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class RifleShotSkill : SkillBase
    {
        private const SkillSid SID = SkillSid.RifleShot;

        public RifleShotSkill() : this(false)
        {
        }

        public RifleShotSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplier = u.Unit.GetEquipmentAttackMultiplier(SID);
                    var res = new DamageEffect
                    {
                        Actor = u,
                        DamageMultiplier = 1f * equipmentMultiplier
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
            SoundEffectType = GameObjectSoundType.Gunshot
        };
    }

    internal class ShotgunSkill : SkillBase
    {
        private const SkillSid SID = SkillSid.Shotgun;

        public ShotgunSkill() : this(false)
        {
        }

        public ShotgunSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.RandomEnemy,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplier = u.Unit.GetEquipmentAttackMultiplier(SID);
                    var res = new DamageEffect
                    {
                        Actor = u,
                        DamageMultiplier = 1.3f * equipmentMultiplier
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
            SoundEffectType = GameObjectSoundType.Gunshot
        };
    }
}