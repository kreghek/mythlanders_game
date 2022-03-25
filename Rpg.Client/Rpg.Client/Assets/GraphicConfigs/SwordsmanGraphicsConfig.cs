using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class SwordsmanGraphicsConfig : UnitGraphicsConfigBase
    {
        public SwordsmanGraphicsConfig()
        {
            Animations = new Dictionary<AnimationSid, AnimationInfo>
            {
                { AnimationSid.Idle, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) },
                { AnimationSid.Defense, new AnimationInfo(startFrame: 16, frames: 1, speed: 0) },
                {
                    AnimationSid.MoveForward, new AnimationInfo(startFrame: 32, frames: 8, speed: 6) { IsFinal = true }
                },
                {
                    AnimationSid.MoveBackward,
                    new AnimationInfo(startFrame: 32, frames: 8, speed: 6) { IsFinal = true }
                },

                { AnimationSid.Skill1, new AnimationInfo(startFrame: 8, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Skill2, new AnimationInfo(startFrame: 24, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Skill3, new AnimationInfo(startFrame: 16, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Skill4, new AnimationInfo(startFrame: 24, frames: 8, speed: 8) { IsFinal = true } },

                { AnimationSid.Ult, new AnimationInfo(startFrame: 59, frames: 2, speed: 8) },

                { AnimationSid.Wound, new AnimationInfo(startFrame: 40, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Death, new AnimationInfo(startFrame: 48, frames: 8, speed: 8) { IsFinal = true } }
            };
        }
    }
}