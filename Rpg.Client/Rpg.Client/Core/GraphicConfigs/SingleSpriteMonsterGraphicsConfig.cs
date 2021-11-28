using System.Collections.Generic;

namespace Rpg.Client.Core.GraphicConfigs
{
    internal sealed class SingleSpriteMonsterGraphicsConfig : UnitGraphicsConfigBase
    {
        public SingleSpriteMonsterGraphicsConfig()
        {
            Animations = new Dictionary<AnimationSid, AnimationInfo>
            {
                { AnimationSid.Idle, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.MoveForward, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.MoveBackward, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Skill1, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Skill2, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Skill3, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Wound, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Death, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) { IsFinal = true } }
            };
        }
    }
}