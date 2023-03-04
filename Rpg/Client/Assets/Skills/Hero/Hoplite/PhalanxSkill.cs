using System.Collections.Generic;
using System.Linq;

using Client;
using Client.GameScreens.Combat.GameObjects;

using Rpg.Client.Assets.TargetSelectors;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Hero.Hoplite
{
    internal class PhalanxSkill : VisualizedSkillBase
    {
        public PhalanxSkill() : this(false)
        {
        }

        private PhalanxSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateProtection(SkillSid.DefenseStance, SkillDirection.Self, 1, 0.75f),
            SkillRuleFactory.CreateProtection(SkillSid.DefenseStance, new LeftAllyTargetSelector(), duration: 1,
                multiplier: 0.75f),
            SkillRuleFactory.CreateProtection(SkillSid.DefenseStance, SkillDirection.Self, duration: 1,
                multiplier: 0.25f, imposeConditions: new[] { new IsRightAllyWithShieldCondition() })
        };

        public override SkillSid Sid => SkillSid.DefenseStance;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.None;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.Defence,
            IconOneBasedIndex = 23,
            AnimationSid = PredefinedAnimationSid.Skill2
        };

        public override IActorVisualizationState CreateState(UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject, AnimationBlocker mainStateBlocker, ISkillVisualizationContext context)
        {
            animatedUnitGameObject.ChangeState(CombatUnitState.Defense);
            return base.CreateState(animatedUnitGameObject, targetUnitGameObject, mainStateBlocker, context);
        }

        private static int? GetRightIndex(int baseIndex)
        {
            return baseIndex switch
            {
                0 => 2,
                1 => 0,
                3 => 5,
                4 => 3,
                _ => null
            };
        }

        private sealed class IsRightAllyWithShieldCondition : IEffectCondition
        {
            public bool Check(ICombatUnit target, CombatEffectContext effectContext)
            {
                var aliveAllies = effectContext.Combat.AliveUnits.Where(x => x.Unit.IsPlayerControlled);

                var selfIndex = ((CombatUnit)target).SlotIndex;

                var targetIndex = GetRightIndex(selfIndex);

                if (targetIndex is null)
                {
                    return false;
                }

                var rightAllyUnit = aliveAllies.SingleOrDefault(x => ((CombatUnit)x).SlotIndex == targetIndex);

                if (rightAllyUnit is not null)
                {
                    return rightAllyUnit.Unit.UnitScheme.Name == UnitName.Assaulter;
                }

                return false;
            }

            public string GetDescription()
            {
                return UiResource.EffectConditionIsRightAllyWithShieldText;
            }
        }
    }
}