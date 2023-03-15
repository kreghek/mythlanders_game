//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Hoplite
//{
//    internal class ContemptSkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.JavelinThrow;

//        public ContemptSkill() : base(PredefinedVisualization, false)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreateDamage(SID, SkillDirection.Target, multiplier: 0.5f),
//            SkillRuleFactory.CreateDamage(SID, SkillDirection.Target, multiplier: 0.5f),
//            new EffectRule
//            {
//                Direction = SkillDirection.Target,
//                EffectCreator = new EffectCreator(_ => new PushBackEffect())
//            }
//        };

//        public override SkillSid Sid => SID;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Melee;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Melee,
//            SoundEffectType = GameObjectSoundType.StaffHit,
//            IconOneBasedIndex = 24
//        };
//    }
//}

