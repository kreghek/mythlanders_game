//using System.Collections.Generic;

//using Client.GameScreens.Combat.GameObjects;

//using Rpg.Client.Core;
//using Rpg.Client.Engine;
//using Rpg.Client.GameScreens;
//using Rpg.Client.GameScreens.Combat.GameObjects;

//namespace Rpg.Client.Assets.Skills.Hero.Swordsman
//{
//    internal class DefenseStanceSkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.DefenseStance;

//        public DefenseStanceSkill() : base(PredefinedVisualization, false)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreateProtection(SID, SkillDirection.Self, 0.5f)
//        };

//        public override SkillSid Sid => SID;
//        public override SkillTargetType TargetType => SkillTargetType.Self;
//        public override SkillType Type => SkillType.None;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Self,
//            SoundEffectType = GameObjectSoundType.Defence,
//            AnimationSid = PredefinedAnimationSid.Skill3,
//            IconOneBasedIndex = 3
//        };

//        public override IActorVisualizationState CreateState(CombatantGameObject animatedUnitGameObject,
//            CombatantGameObject targetUnitGameObject, AnimationBlocker mainStateBlocker, ISkillVisualizationContext context)
//        {
//            animatedUnitGameObject.ChangeState(CombatUnitState.Defense);
//            return base.CreateState(animatedUnitGameObject, targetUnitGameObject, mainStateBlocker, context);
//        }
//    }
//}

