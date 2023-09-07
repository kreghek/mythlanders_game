//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Medjay
//{
//    internal class SuperNaturalAgilitySkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.SuperNaturalAgility;

//        public SuperNaturalAgilitySkill() : base(PredefinedVisualization, false)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreateProtection(SID, SkillDirection.Self, duration: 1, multiplier: 0.5f)
//        };

//        public override SkillSid Sid => SID;
//        public override SkillTargetType TargetType => SkillTargetType.Self;
//        public override SkillType Type => SkillType.None;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Self,
//            SoundEffectType = GameObjectSoundType.Defence,
//            IconOneBasedIndex = 31
//        };
//    }
//}

