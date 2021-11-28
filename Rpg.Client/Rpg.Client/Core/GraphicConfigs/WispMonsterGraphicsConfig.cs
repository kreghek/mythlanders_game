using System.Collections.Generic;

namespace Rpg.Client.Core.GraphicConfigs
{
    internal sealed class WispMonsterGraphicsConfig : UnitGraphicsConfigBase
    {
        public WispMonsterGraphicsConfig()
        {
            Animations = new Dictionary<AnimationSid, AnimationInfo>
            {
                { AnimationSid.Idle, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) },
                { AnimationSid.MoveForward, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.MoveBackward, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Skill1, new AnimationInfo(startFrame: 8, frames: 4, speed: 4) },
                { AnimationSid.Skill2, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Skill3, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Wound, new AnimationInfo(startFrame: 0, frames: 2, speed: 8) },
                { AnimationSid.Death, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) { IsFinal = true } }
            };
        }
    }
}