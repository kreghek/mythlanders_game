﻿//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Monster
//{
//    internal class HealSkill : VisualizedSkillBase
//    {
//        public HealSkill() : this(false)
//        {
//        }

//        public HealSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            new EffectRule
//            {
//                Direction = SkillDirection.Target,
//                EffectCreator = new EffectCreator(u =>
//                {
//                    var res = new HealEffect(u)
//                    {
//                        PowerMultiplier = 1
//                    };

//                    return res;
//                })
//            }
//        };

//        public override SkillSid Sid => SkillSid.Heal;
//        public override SkillTargetType TargetType => SkillTargetType.Friendly;
//        public override SkillType Type => SkillType.Melee;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Self,
//            SoundEffectType = GameObjectSoundType.Heal
//        };
//    }
//}

