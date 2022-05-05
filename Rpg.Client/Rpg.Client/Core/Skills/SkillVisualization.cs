﻿using Rpg.Client.GameScreens;

namespace Rpg.Client.Core.Skills
{
    internal struct SkillVisualization
    {
        public SkillVisualizationStateType Type { get; init; }
        public GameObjectSoundType SoundEffectType { get; init; }
        public AnimationSid AnimationSid { get; init; }
        public int IconOneBasedIndex { get; init; }
    }
}