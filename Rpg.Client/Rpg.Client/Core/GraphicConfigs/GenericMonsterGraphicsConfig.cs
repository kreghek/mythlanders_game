using System.Collections.Generic;

namespace Rpg.Client.Core.GraphicConfigs
{
    internal sealed class GenericMonsterGraphicsConfig : UnitGraphicsConfigBase
    {
        public GenericMonsterGraphicsConfig()
        {
            Animations = new Dictionary<AnimationSid, AnimationInfo>
            {
                { AnimationSid.Idle, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) },
                { AnimationSid.MoveForward, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.MoveBackward, new AnimationInfo(startFrame: 16, frames: 8, speed: 6) { IsFinal = true } },
                { AnimationSid.Skill1, new AnimationInfo(startFrame: 8, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Skill2, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Skill3, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Wound, new AnimationInfo(startFrame: 24, frames: 8, speed: 8) },
                { AnimationSid.Death, new AnimationInfo(startFrame: 32, frames: 8, speed: 8) { IsFinal = true } }
            };
        }
    }
}