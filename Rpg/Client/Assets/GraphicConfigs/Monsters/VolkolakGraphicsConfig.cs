﻿using System.Collections.Generic;

using Client.Assets.GraphicConfigs.Monsters;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs.Monsters
{
    internal sealed class VolkolakGraphicsConfig : SlavicMonsterGraphicConfig
    {
        public VolkolakGraphicsConfig()
        {
            StatsPanelOrigin = new Vector2(-16, 0);
        }

        public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
        {
            return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
            {
                { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle(startFrameIndex: 40) },
                {
                    PredefinedAnimationSid.MoveForward,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 40, frameCount: 1, fps: 1)
                },
                {
                    PredefinedAnimationSid.MoveBackward,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, fps: 6)
                },
                {
                    PredefinedAnimationSid.Skill1,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 56, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Skill2,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
                },
                {
                    PredefinedAnimationSid.Skill3,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 0, frameCount: 1, fps: 1)
                },
                {
                    PredefinedAnimationSid.Wound,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 64, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Death,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 72, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.ShapeShift,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 16, fps: 8)
                }
            };
        }
    }
}