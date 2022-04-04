using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class SwordSlashRandomSkill : SkillBase
    {
        private const SkillSid SID = SkillSid.SwordSlashRandom;

        public SwordSlashRandomSkill() : this(false)
        {
        }

        public SwordSlashRandomSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
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
                        DamageMultiplier = 1 * equipmentMultiplier
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        public override int Weight => BASE_WEIGHT * 2;

        private static SkillVisualization PredefinedVisualization => new()
        {
            AnimationSid = AnimationSid.Skill1,
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.SwordSlash
        };
    }
}