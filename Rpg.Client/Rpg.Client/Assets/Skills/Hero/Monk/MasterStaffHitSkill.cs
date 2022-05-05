using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Monk
{
    internal class MasterStaffHitSkill : VisualizedSkillBase
    {
        private static readonly SkillVisualization _predefinedSkillVisualization = new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.StaffHit,
            IconOneBasedIndex = 17
        };

        public MasterStaffHitSkill(bool costRequired) : base(_predefinedSkillVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
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

        public override SkillSid Sid => SkillSid.MasterStaffHit;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;
    }
}