using System;
using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Sergeant
{
    internal sealed class InspiringExampleSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.InspiringExample;

        private static SkillVisualization _predefinedSkillVisualization = new SkillVisualization { 
            AnimationSid = Core.AnimationSid.Skill1,
            SoundEffectType = GameScreens.GameObjectSoundType.SwordSlash,
            Type = SkillVisualizationStateType.Melee
        };

        public InspiringExampleSkill() : base(_predefinedSkillVisualization)
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
                        DamageMultiplier = 1 * equipmentMultiplier
                    };

                    return res;
                })
            },
            new EffectRule {
                Direction = SkillDirection.RandomFriendly,
                EffectCreator = new EffectCreator(u =>{
                    var res = new IncreaseAttackEffect(5)
                    { 
                        
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;
    }
}
