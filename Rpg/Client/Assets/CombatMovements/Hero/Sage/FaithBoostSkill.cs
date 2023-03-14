//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Sage
//{
//    internal sealed class FaithBoostSkill : VisualizedSkillBase
//    {
//        public FaithBoostSkill() : base(PredefinedVisualization, false)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreatePowerUp(SkillSid.FaithBoost, SkillDirection.Target, 3)
//        };

//        public override SkillSid Sid => SkillSid.FaithBoost;
//        public override SkillTargetType TargetType => SkillTargetType.Friendly;
//        public override SkillType Type => SkillType.Range;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Self,
//            SoundEffectType = GameObjectSoundType.MagicDust
//        };
//    }
//}