﻿using System.Collections.Generic;

using Client.Core;
using Client.Core.AnimationFrameSets;

namespace Client.Assets.GraphicConfigs.Monsters;

internal sealed class ChaserGraphicConfig: EgyptianMonsterGraphicConfig
{
    public ChaserGraphicConfig() : base(UnitName.Chaser)
    {
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            { PredefinedAnimationSid.Idle, new LinearAnimationFrameSet(new[]{0}, 0, frameWidth: 128, frameHeight: 156, textureColumns: 3) },
            {
                PredefinedAnimationSid.MoveForward,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
            },
            {
                PredefinedAnimationSid.MoveBackward,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, fps: 6)
            },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, fps: 8)
            }
        };
    }
}