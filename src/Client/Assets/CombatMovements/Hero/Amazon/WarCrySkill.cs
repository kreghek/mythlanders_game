﻿//using System.Collections.Generic;

//using JetBrains.Annotations;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Amazon
//{
//    [UsedImplicitly]
//    internal sealed class WarCrySkill : VisualizedSkillBase
//    {
//        public WarCrySkill() : base(PredefinedVisualization, false)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreatePowerUp(SkillSid.WarCry, SkillDirection.AllFriendly, 3)
//        };

//        public override SkillSid Sid => SkillSid.WarCry;
//        public override SkillTargetType TargetType => SkillTargetType.Friendly;
//        public override SkillType Type => SkillType.Range;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Self,
//            SoundEffectType = GameObjectSoundType.AmazonWarCry,
//            IconOneBasedIndex = 28
//        };
//    }
//}

