//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Liberator
//{
//    internal sealed class MotivationSkill : VisualizedSkillBase
//    {
//        public MotivationSkill() : this(false)
//        {
//        }

//        public MotivationSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreatePowerUp(SkillSid.Motivation, SkillDirection.Target, 1)
//        };

//        public override SkillSid Sid => SkillSid.Motivation;
//        public override SkillTargetType TargetType => SkillTargetType.Friendly;
//        public override SkillType Type => SkillType.Range;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Self,
//            SoundEffectType = GameObjectSoundType.MagicDust
//        };
//    }
//}