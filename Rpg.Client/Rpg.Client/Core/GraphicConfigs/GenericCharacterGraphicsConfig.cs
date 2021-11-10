using System.Collections.Generic;

namespace Rpg.Client.Core.GraphicConfigs
{
    internal sealed class GenericCharacterGraphicsConfig : UnitGraphicsConfigBase
    {
        public GenericCharacterGraphicsConfig()
        {
            Animations = new Dictionary<string, AnimationInfo>
            {
                { DEFAULT_ANIMATION_SID, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) },
                { "Skill1", new AnimationInfo(startFrame: 8, frames: 8, speed: 8) { IsFinal = true } },
                { "Skill2", new AnimationInfo(startFrame: 16, frames: 8, speed: 8) { IsFinal = true } },
                { "Skill3", new AnimationInfo(startFrame: 24, frames: 8, speed: 8) { IsFinal = true } },
                { "Wound", new AnimationInfo(startFrame: 32, frames: 8, speed: 8) { IsFinal = true } },
                { "Death", new AnimationInfo(startFrame: 40, frames: 8, speed: 8) { IsFinal = true } }
            };
        }
    }
}