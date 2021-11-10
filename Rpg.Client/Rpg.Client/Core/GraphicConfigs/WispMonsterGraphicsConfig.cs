using System.Collections.Generic;

namespace Rpg.Client.Core.GraphicConfigs
{
    internal sealed class WispMonsterGraphicsConfig : UnitGraphicsConfigBase
    {
        public WispMonsterGraphicsConfig()
        {
            Animations = new Dictionary<string, AnimationInfo>
                    {
                        { DEFAULT_ANIMATION_SID, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) },
                        { "MoveForward", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "MoveBackward", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "Skill1", new AnimationInfo(startFrame: 8, frames: 4, speed: 4) },
                        { "Skill2", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "Skill3", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "Wound", new AnimationInfo(startFrame: 0, frames: 2, speed: 8) },
                        { "Death", new AnimationInfo(startFrame: 0, frames: 8, speed: 8) { IsFinal = true } }
                    };
        }
    }
}