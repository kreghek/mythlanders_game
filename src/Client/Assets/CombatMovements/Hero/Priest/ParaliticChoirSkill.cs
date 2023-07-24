//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Priest
//{
//    internal class ParalyticChoirSkill : VisualizedSkillBase
//    {
//        public ParalyticChoirSkill() : base(PredefinedVisualization, false)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            new EffectRule
//            {
//                Direction = SkillDirection.AllEnemies,
//                EffectCreator = new EffectCreator(u =>
//                {
//                    var effect = new StunEffect(u);

//                    return effect;
//                })
//            }
//        };

//        public override SkillSid Sid => SkillSid.ParalyticChoir;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Range;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Self,
//            SoundEffectType = GameObjectSoundType.EgyptianDarkMagic,
//            IconOneBasedIndex = 36
//        };
//    }
//}