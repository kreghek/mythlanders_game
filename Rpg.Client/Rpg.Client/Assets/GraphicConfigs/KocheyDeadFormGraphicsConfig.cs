using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class KocheyDeadFormGraphicsConfig : UnitGraphicsConfigBase
    {
        public KocheyDeadFormGraphicsConfig()
        {
            Animations = new Dictionary<AnimationSid, AnimationInfo>
            {
                { AnimationSid.Idle, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.MoveForward, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.MoveBackward, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Skill1, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Skill2, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Skill3, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Skill4, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Ult, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.ShapeShift, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Wound, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { AnimationSid.Death, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) { IsFinal = true } },
                { AnimationSid.Defense, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) }
            };
        }
    }
}