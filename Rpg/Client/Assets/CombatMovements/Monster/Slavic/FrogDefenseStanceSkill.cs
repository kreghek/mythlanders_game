//using System.Collections.Generic;

//using Rpg.Client.Core;
//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Monster.Slavic
//{
//    internal class FrogDefenseStanceSkill : VisualizedSkillBase
//    {
//        public FrogDefenseStanceSkill() : this(false)
//        {
//        }

//        public FrogDefenseStanceSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreateProtection(SkillSid.DefenseStance, SkillDirection.Self, 0.5f)
//        };

//        public override SkillSid Sid => SkillSid.DefenseStance;
//        public override SkillTargetType TargetType => SkillTargetType.Self;
//        public override SkillType Type => SkillType.None;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Self,
//            SoundEffectType = GameObjectSoundType.FrogHornsUp,
//            AnimationSid = PredefinedAnimationSid.Skill3
//        };
//    }
//}