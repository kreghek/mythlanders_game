using System.Collections.Generic;

namespace Rpg.Client.Core.GraphicConfigs
{
    internal sealed class GenericMonsterGraphicsConfig : UnitGraphicsConfigBase
    {
        public GenericMonsterGraphicsConfig()
        {
            Animations = new Dictionary<string, AnimationInfo>
                    {
                        { DEFAULT_ANIMATION_SID, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) },
                        { "MoveForward", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "MoveBackward", new AnimationInfo(startFrame: 16, frames: 8, speed: 6) { IsFinal = true } },
                        { "Skill1", new AnimationInfo(startFrame: 8, frames: 8, speed: 8) { IsFinal = true } },
                        { "Skill2", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "Skill3", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "Wound", new AnimationInfo(startFrame: 24, frames: 8, speed: 8) },
                        { "Death", new AnimationInfo(startFrame: 32, frames: 8, speed: 8) { IsFinal = true } }
                    };
        }
    }
}