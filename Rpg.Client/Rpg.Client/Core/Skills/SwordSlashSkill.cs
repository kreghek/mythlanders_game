using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal class SwordSlashSkill : SkillBase
    {
        private const SkillSid SID = SkillSid.SwordSlash;

        public SwordSlashSkill() : this(false)
        {
        }

        public SwordSlashSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplier = u.Unit.GetEquipmentAttackMultiplier(SID);

                    var res = new AttackEffect
                    {
                        Actor = u,
                        DamageMultiplier = 1 * equipmentMultiplier
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SkillSid.SwordSlash;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        public override int UsageCount => 3;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.SwordSlash
        };
    }
    
    internal class MummificationTouchSkill : SkillBase
    {
        private const SkillSid SID = SkillSid.SwordSlash;

        public MummificationTouchSkill() : this(false)
        {
        }

        public MummificationTouchSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var equipmentMultiplier = u.Unit.GetEquipmentAttackMultiplier(SID);

                    var res = new AttackEffect
                    {
                        Actor = u,
                        DamageMultiplier = 1 * equipmentMultiplier
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SkillSid.MummificationTouch;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        public override int UsageCount => 3;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.SwordSlash
        };
    }
}