using System.Collections.Generic;

using Client.GameScreens.Combat.GameObjects;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat.GameObjects;
using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates;

namespace Rpg.Client.Assets.Skills.Hero.Commissar
{
    internal class TacticalManeuverSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.TacticalManeuver;

        public TacticalManeuverSkill() : this(false)
        {
        }

        private TacticalManeuverSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    return new ExchangeSlotEffect
                    {
                        Actor = (CombatUnit)u
                    };
                })
            },
            SkillRuleFactory.CreateProtection(SID, SkillDirection.Target, 1, 0.25f)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.Gunshot,
            IconOneBasedIndex = 39
        };

        public override IActorVisualizationState CreateState(CombatantGameObject animatedUnitGameObject,
            CombatantGameObject targetUnitGameObject, AnimationBlocker mainStateBlocker, ISkillVisualizationContext context)
        {
            foreach (var interaction in context.Interaction.SkillRuleInteractions)
            {
                foreach (var target in interaction.Targets)
                {
                    interaction.Action(target);
                }
            }

            return new EmptyState(mainStateBlocker);
        }
    }
}