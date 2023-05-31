//using System.Collections.Generic;

//using Microsoft.Xna.Framework;

//using Rpg.Client.Assets.InteractionDeliveryObjects;
//using Rpg.Client.Core;
//using Rpg.Client.Engine;
//using Rpg.Client.GameScreens;
//using Rpg.Client.GameScreens.Combat.GameObjects;

//namespace Client.Assets.CombatMovements.Monster.Black
//{
//    internal class BlackRifleShotSkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.BlackRifleShot;

//        public BlackRifleShotSkill() : this(false)
//        {
//        }

//        private BlackRifleShotSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreateDamage(SID)
//        };

//        public override SkillSid Sid => SID;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Range;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Range,
//            SoundEffectType = GameObjectSoundType.Gunshot,
//            AnimationSid = PredefinedAnimationSid.Skill1
//        };

//        protected override IInteractionDelivery CreateProjectile(Vector2 startPosition, Vector2 targetPosition,
//            ISkillVisualizationContext context, AnimationBlocker bulletAnimationBlocker)
//        {
//            var singleInteractionDelivery = new KineticBulletProjectile(
//                startPosition,
//                targetPosition,
//                context.GameObjectContentStorage,
//                bulletAnimationBlocker);

//            return singleInteractionDelivery;
//        }
//    }
//}