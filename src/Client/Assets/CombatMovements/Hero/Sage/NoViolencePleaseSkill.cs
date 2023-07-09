//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Sage
//{
//    internal class NoViolencePleaseSkill : VisualizedSkillBase
//    {
//        public NoViolencePleaseSkill() : this(false)
//        {
//        }

//        public NoViolencePleaseSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            new EffectRule
//            {
//                Direction = SkillDirection.Target,
//                EffectCreator = new EffectCreator(u =>
//                {
//                    var effect = new StunEffect(u);

//                    return effect;
//                })
//            }
//        };

//        public override SkillSid Sid => SkillSid.NoViolencePlease;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Range;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Self,
//            SoundEffectType = GameObjectSoundType.MagicDust
//        };
//    }
//}