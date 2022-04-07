using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Skills
{
    internal class StaffHitSkill : SkillBase
    {
        public StaffHitSkill() : this(false)
        {
        }

        public StaffHitSkill(bool costRequired) : base(new SkillVisualization
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.StaffHit,
            AnimationSid = Core.AnimationSid.Skill1
        }, costRequired)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new DamageEffect
                    {
                        Actor = u,
                        DamageMultiplier = 0.25f
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SkillSid.StaffHit;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;
    }
}