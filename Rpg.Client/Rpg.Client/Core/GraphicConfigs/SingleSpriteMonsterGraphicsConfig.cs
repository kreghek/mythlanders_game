using System.Collections.Generic;

namespace Rpg.Client.Core.GraphicConfigs
{
    internal sealed class SingleSpriteMonsterGraphicsConfig : UnitGraphicsConfigBase
    {
        public SingleSpriteMonsterGraphicsConfig()
        {
            Animations = new Dictionary<string, AnimationInfo>
            {
                { DEFAULT_ANIMATION_SID, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { "MoveForward", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { "MoveBackward", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { "Skill1", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { "Skill2", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { "Skill3", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { "Wound", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                { "Death", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) { IsFinal = true } }
            };
        }
    }
}