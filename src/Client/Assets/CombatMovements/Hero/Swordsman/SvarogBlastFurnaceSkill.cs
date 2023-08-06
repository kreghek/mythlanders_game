//using System.Collections.Generic;

//using Client.GameScreens.Combat.GameObjects;

//using Rpg.Client.Assets.States.HeroSpecific;
//using Rpg.Client.Core;
//using Rpg.Client.Engine;
//using Rpg.Client.GameScreens;
//using Rpg.Client.GameScreens.Combat.GameObjects;

//namespace Rpg.Client.Assets.Skills.Hero.Swordsman
//{
//    internal class SvarogBlastFurnaceSkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.SvarogBlastFurnace;

//        public SvarogBlastFurnaceSkill() : base(PredefinedVisualization, false)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreateDamage(SID, SkillDirection.AllEnemies, multiplier: 1.5f)
//        };

//        public override SkillSid Sid => SID;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Range;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.MassRange,
//            SoundEffectType = GameObjectSoundType.FireDamage,
//            AnimationSid = PredefinedAnimationSid.Skill4,
//            IconOneBasedIndex = 4
//        };

//        public override IActorVisualizationState CreateState(
//            CombatantGameObject animatedUnitGameObject,
//            CombatantGameObject targetUnitGameObject,
//            AnimationBlocker mainAnimationBlocker,
//            ISkillVisualizationContext context)
//        {
//            var state = new SvarogFurnaceBlastUsageState(animatedUnitGameObject, mainAnimationBlocker,
//                context.Interaction,
//                context.InteractionDeliveryManager, context.GameObjectContentStorage, context.AnimationManager,
//                context.GetSoundEffect(GameObjectSoundType.SvarogSymbolAppearing),
//                context.GetSoundEffect(GameObjectSoundType.RisingPower),
//                context.GetSoundEffect(GameObjectSoundType.Firestorm),
//                context.GetSoundEffect(GameObjectSoundType.FireDamage),
//                context.ScreenShaker);

//            return state;
//        }
//    }
//}

