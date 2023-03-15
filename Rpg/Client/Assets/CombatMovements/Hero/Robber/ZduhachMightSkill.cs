//using System.Collections.Generic;

//using JetBrains.Annotations;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Archer
//{
//    [UsedImplicitly]
//    internal class ZduhachMightSkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.ZduhachMight;

//        public ZduhachMightSkill() : base(PredefinedVisualization, false)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            new EffectRule
//            {
//                Direction = SkillDirection.AllEnemies,
//                EffectCreator = new EffectCreator(u =>
//                {
//                    return new ShieldPointModifyEffect(u, new DurationEffectLifetime(new EffectDuration(3)), 1f);
//                })
//            }
//        };

//        public override SkillSid Sid => SID;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.None;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Self,
//            SoundEffectType = GameObjectSoundType.Defence,
//            IconOneBasedIndex = 8
//        };
//    }
//}