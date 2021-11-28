﻿using System.Collections.Generic;

namespace Rpg.Client.Core.GraphicConfigs
{
    internal sealed class BerimirGraphicsConfig : UnitGraphicsConfigBase
    {
        public BerimirGraphicsConfig()
        {
            Animations = new Dictionary<string, AnimationInfo>
            {
                { DEFAULT_ANIMATION_SID, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) },
                { "Defense", new AnimationInfo(startFrame: 16, frames: 1, speed: 0) },
                {
                    "MoveForward", new AnimationInfo(startFrame: 32, frames: 8, speed: 6) { IsFinal = true }
                },
                {
                    "MoveBackward",
                    new AnimationInfo(startFrame: 32, frames: 8, speed: 6) { IsFinal = true }
                },
                { "Skill1", new AnimationInfo(startFrame: 8, frames: 8, speed: 8) { IsFinal = true } },
                { "Skill2", new AnimationInfo(startFrame: 16, frames: 8, speed: 8) { IsFinal = true } },
                { "Skill3", new AnimationInfo(startFrame: 24, frames: 8, speed: 8) { IsFinal = true } },
                { "Wound", new AnimationInfo(startFrame: 40, frames: 8, speed: 8) { IsFinal = true } },
                { "Death", new AnimationInfo(startFrame: 48, frames: 8, speed: 8) { IsFinal = true } },
                { "Ult", new AnimationInfo(startFrame: 59, frames: 2, speed: 8) }
            };
        }
    }
}