﻿using System.Collections.Generic;

using Client.Core;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;

namespace Client.Assets.GraphicConfigs.Monsters.Egyptian;

internal sealed class ChaserGraphicConfig : EgyptianMonsterGraphicConfig
{
    public ChaserGraphicConfig() : base(UnitName.Chaser)
    {
        Origin = new Vector2(80, 110);
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            {
                PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle()
            },
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
                AnimationFrameSetFactory.CreateIdleFromGrid(new[]{ 3 })
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateIdleFromGrid(new[]{ 4 })
            }
        };
    }
}