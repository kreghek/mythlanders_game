﻿using System.Collections.Generic;

namespace Rpg.Client.Core.GraphicConfigs
{
    internal sealed class HawkGraphicsConfig : UnitGraphicsConfigBase
    {
        public HawkGraphicsConfig()
        {
            Animations = new Dictionary<AnimationSid, AnimationInfo>
            {
                { AnimationSid.Idle, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) },
                { AnimationSid.Skill1, new AnimationInfo(startFrame: 8, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Skill2, new AnimationInfo(startFrame: 16, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Skill3, new AnimationInfo(startFrame: 24, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Wound, new AnimationInfo(startFrame: 32, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Death, new AnimationInfo(startFrame: 40, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Defense, new AnimationInfo(startFrame: 16, frames: 1, speed: 1) }
            };
        }
    }
}