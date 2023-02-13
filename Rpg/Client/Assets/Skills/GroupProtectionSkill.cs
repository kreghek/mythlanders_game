﻿using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills
{
    internal class GroupProtectionSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.GroupProtection;

        public GroupProtectionSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateProtection(SID, SkillDirection.AllFriendly, 0.5f)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Self;
        public override SkillType Type => SkillType.None;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Self,
            SoundEffectType = GameObjectSoundType.Defence,
            AnimationSid = PredefinedAnimationSid.Skill3,
            IconOneBasedIndex = 15
        };
    }
}