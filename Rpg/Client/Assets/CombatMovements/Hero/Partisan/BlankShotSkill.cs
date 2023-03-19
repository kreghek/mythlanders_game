//using System.Collections.Generic;

//using Microsoft.Xna.Framework;

//using Rpg.Client.Assets.InteractionDeliveryObjects;
//using Rpg.Client.Engine;
//using Rpg.Client.GameScreens;
//using Rpg.Client.GameScreens.Combat.GameObjects;

//namespace Rpg.Client.Assets.Skills.Hero.Commissar
//{
//    internal class BlankShotSkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.BlankShot;

//        public BlankShotSkill() : this(false)
//        {
//        }

//        private BlankShotSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
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
//            IconOneBasedIndex = 40
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