using System.Collections.Generic;

namespace Rpg.Client.Core.GraphicConfigs
{
    internal sealed class VolkolakGraphicsConfig : UnitGraphicsConfigBase
    {
        public VolkolakGraphicsConfig()
        {
            Animations = new Dictionary<AnimationSid, AnimationInfo>
            {
                { AnimationSid.Idle, new AnimationInfo(startFrame: 40, frames: 8, speed: 8) },
                { AnimationSid.MoveForward, new AnimationInfo(startFrame: 40, frames: 1, speed: 1) },
                {
                    AnimationSid.MoveBackward, new AnimationInfo(startFrame: 16, frames: 8, speed: 6) { IsFinal = true }
                },
                { AnimationSid.Skill1, new AnimationInfo(startFrame: 56, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Skill2, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Skill3, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Wound, new AnimationInfo(startFrame: 64, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Death, new AnimationInfo(startFrame: 72, frames: 8, speed: 8) { IsFinal = true } }
            };
        }
    }
}