using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Sergeant
{
    internal class InspiringRushSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.InspiringRush;

        public InspiringRushSkill() : base(PredefinedVisualization, false)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
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
                        DamageMultiplier = 1 * equipmentMultiplier
                    };

                    return res;
                })
            },
            
            SkillRuleFactory.CreatePowerUp(SID, 1, SkillDirection.RandomFriendly)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            AnimationSid = AnimationSid.Skill1,
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.SwordSlash
        };
    }
}