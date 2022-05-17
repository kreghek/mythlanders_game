using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Hero.Swordsman
{
    internal class DefenseStanceSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.DefenseStance;

        public DefenseStanceSkill() : this(false)
        {
        }

        public DefenseStanceSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateProtection(SID, SkillDirection.Self, 0.5f)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.None;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.Defence,
            AnimationSid = PredefinedAnimationSid.Skill3,
            IconOneBasedIndex = 3
        };

        public override IUnitStateEngine CreateState(UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject, AnimationBlocker mainStateBlocker, ISkillVisualizationContext context)
        {
            animatedUnitGameObject.CombatUnit.ChangeState(CombatUnitState.Defense);
            return base.CreateState(animatedUnitGameObject, targetUnitGameObject, mainStateBlocker, context);
        }
    }
}