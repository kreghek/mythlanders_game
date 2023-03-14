//using System.Collections.Generic;

//using JetBrains.Annotations;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Engineer
//{
//    [UsedImplicitly]
//    internal class DismantlementSkill : VisualizedSkillBase
//    {
//        public DismantlementSkill() : base(PredefinedVisualization, false)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            new EffectRule
//            {
//                Direction = SkillDirection.Target,
//                EffectCreator = new EffectCreator(u =>
//                {
//                    var effect = new StunEffect(u, new DurationEffectLifetime(new EffectDuration(3)));

//                    return effect;
//                })
//            }
//        };

//        public override SkillSid Sid => SkillSid.Demountage;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Range;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Self,
//            SoundEffectType = GameObjectSoundType.MagicDust
//        };
//    }
//}