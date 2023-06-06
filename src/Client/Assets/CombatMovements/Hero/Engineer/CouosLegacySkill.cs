//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Engineer
//{
//    internal class CouosLegacySkill : VisualizedSkillBase
//    {
//        public CouosLegacySkill() : base(PredefinedVisualization, false)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreateProtection(SkillSid.CouosLegacy, SkillDirection.AllFriendly, 0.95f)
//        };

//        public override SkillSid Sid => SkillSid.CouosLegacy;
//        public override SkillTargetType TargetType => SkillTargetType.Self;
//        public override SkillType Type => SkillType.None;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Self,
//            SoundEffectType = GameObjectSoundType.Defence
//        };
//    }
//}

