using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs
{
    internal sealed class VolkolakWarriorGraphicsConfig : UnitGraphicsConfigBase
    {
        public VolkolakWarriorGraphicsConfig()
        {
            Animations = new Dictionary<AnimationSid, AnimationInfo>
            {
                { AnimationSid.Idle, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) },
                { AnimationSid.Skill1, new AnimationInfo(startFrame: 8, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Wound, new AnimationInfo(startFrame: 16, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.Death, new AnimationInfo(startFrame: 24, frames: 8, speed: 8) { IsFinal = true } },
                { AnimationSid.ShapeShift, new AnimationInfo(startFrame: 24, frames: 16, speed: 8) { IsFinal = true } }
            };
        }
    }
}