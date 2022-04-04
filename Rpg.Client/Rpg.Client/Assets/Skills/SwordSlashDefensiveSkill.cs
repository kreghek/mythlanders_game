using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class SwordSlashDefensiveSkill : SkillBase
    {
        private const SkillSid SID = SkillSid.SwordSlashDefensive;

        public override int Weight => BASE_WEIGHT * 2;

        public SwordSlashDefensiveSkill() : this(false)
        {
        }

        public SwordSlashDefensiveSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
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

                    var res = new DamageEffect
                    {
                        Actor = u,
                        DamageMultiplier = 0.5f * equipmentMultiplier
                    };

                    return res;
                })
            },

            new EffectRule
            {
                Direction = SkillDirection.Self,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new DecreaseDamageEffect(0.3f)
                    {
                        Duration = 1
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            AnimationSid = Core.AnimationSid.Skill1,
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.SwordSlash
        };
    }
}