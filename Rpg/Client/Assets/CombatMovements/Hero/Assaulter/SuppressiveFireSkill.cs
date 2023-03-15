//using System.Collections.Generic;

//using Client.GameScreens.Combat.GameObjects;

//using Rpg.Client.Engine;
//using Rpg.Client.GameScreens;
//using Rpg.Client.GameScreens.Combat.GameObjects;

//namespace Rpg.Client.Assets.Skills.Hero.Assaulter
//{
//    internal class SuppressiveFireSkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.SuppressiveFire;

//        private static readonly int[] _penalties =
//        {
//            2,
//            4,
//            6,
//            10,
//            15
//        };

//        public SuppressiveFireSkill() : this(false)
//        {
//        }

//        private SuppressiveFireSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = CreateRules();

//        public override SkillSid Sid => SID;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Range;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Range,
//            SoundEffectType = GameObjectSoundType.Gunshot,
//            IconOneBasedIndex = 16
//        };

//        public override IActorVisualizationState CreateState(CombatantGameObject animatedUnitGameObject,
//            CombatantGameObject targetUnitGameObject,
//            AnimationBlocker mainStateBlocker, ISkillVisualizationContext context)
//        {
//            //var mainShootingBlocker = context.AddAnimationBlocker();
//            //var interactionItems = context.Interaction.SkillRuleInteractions
//            //    .Where(x => (x.Metadata is AssaultSkillRuleMetadata meta) && meta.IsShot).ToArray();
//            //var bulletDataList = new List<(AnimationBlocker, IInteractionDelivery)>();
//            //foreach (var item in interactionItems)
//            //{
//            //    var bulletAnimationBlocker = context.AddAnimationBlocker();

//            //    var materializedTarget = item.Targets[0];
//            //    var materializedTargetGameObject = context.GetGameObject(materializedTarget);
//            //    var materializedTargetGameObjectPosition = materializedTargetGameObject.InteractionPoint;

//            //    var singleInteractionDelivery = new KineticBulletProjectile(animatedUnitGameObject.LaunchPoint,
//            //        materializedTargetGameObjectPosition,
//            //        context.GameObjectContentStorage,
//            //        bulletAnimationBlocker,
//            //        materializedTarget,
//            //        item.Action);

//            //    bulletDataList.Add(new(bulletAnimationBlocker, singleInteractionDelivery));

//            //    bulletAnimationBlocker.Released += (_, _) =>
//            //    {
//            //        var allBulletBlockerIsReleased = bulletDataList.All(x => x.Item1.IsReleased);
//            //        if (allBulletBlockerIsReleased)
//            //        {
//            //            mainShootingBlocker.Release();
//            //        }
//            //    };
//            //}

//            //var animationBlocker = context.AnimationManager.CreateAndRegisterBlocker();

//            //StateHelper.HandleStateWithInteractionDelivery(
//            //    context.Interaction.SkillRuleInteractions.First(x =>
//            //        (x.Metadata is AssaultSkillRuleMetadata { IsBuff: true })),
//            //    mainStateBlocker,
//            //    mainShootingBlocker,
//            //    animationBlocker);

//            //var state = new AssaultRifleBurstState(
//            //    graphics: animatedUnitGameObject.Graphics,
//            //    animationBlocker,
//            //    bulletDataList.Select(x => x.Item2).ToList(),
//            //    interactionDeliveryManager: context.InteractionDeliveryManager,
//            //    rifleShotSound: context.GetSoundEffect(GameObjectSoundType.AssaultRifleBurst),
//            //    animationSid: PredefinedAnimationSid.Skill1);

//            //return state;

//            throw new System.Exception();
//        }

//        private static List<EffectRule> CreateRules()
//        {
//            var list = new List<EffectRule>();

//            var buffEffect = SkillRuleFactory.CreatePowerDown(SID, SkillDirection.AllLineEnemies, 1, _penalties[0]);
//            buffEffect.EffectMetadata = new AssaultSkillRuleMetadata
//            {
//                IsBuff = true
//            };
//            list.Add(buffEffect);

//            for (var i = 0; i < 5; i++)
//            {
//                var rule = SkillRuleFactory.CreateDamage(SID, SkillDirection.RandomLineEnemy, multiplier: 0.2f,
//                    scatter: 0.3f);
//                rule.EffectMetadata = new AssaultSkillRuleMetadata
//                {
//                    IsShot = true
//                };
//                list.Add(rule);
//            }

//            return list;
//        }
//    }
//}