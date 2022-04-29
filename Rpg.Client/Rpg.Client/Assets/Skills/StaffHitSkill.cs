using System.Collections.Generic;

using Rpg.Client.Assets.States;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills
{
    internal class StaffHitSkill : VisualizedSkillBase
    {
        public StaffHitSkill() : this(false)
        {
        }

        public StaffHitSkill(bool costRequired) : base(new SkillVisualization
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.StaffHit,
            AnimationSid = AnimationSid.Skill1
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

        public override IUnitStateEngine CreateState(UnitGameObject animatedUnitGameObject, UnitGameObject targetUnitGameObject,
            ISkillVisualizationContext context)
        {
            var mainAnimationBlocker = context.AnimationManager.CreateAndUseBlocker();
            var state = new MonkTripleHitState(animatedUnitGameObject._graphics, targetUnitGameObject._graphics,
                mainAnimationBlocker, context.Interaction, context.GetHitSound(GameObjectSoundType.StaffHit));
            return state;
        }
    }
}