﻿using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs.Monsters
{
    internal sealed class DigitalWolfGraphicsConfig : UnitGraphicsConfigBase
    {
        public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
        {
            return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
            {
                { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdleFromGrid(rows: new[] { 0, 1, 2 }) },
                {
                    PredefinedAnimationSid.MoveForward,
                    AnimationFrameSetFactory.CreateEmpty()
                },
                {
                    PredefinedAnimationSid.MoveBackward,
                    AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 5 }, fps: 6)
                },
                {
                    PredefinedAnimationSid.Skill1,
                    AnimationFrameSetFactory.CreateEmpty()
                },
                {
                    PredefinedAnimationSid.Skill2,
                    AnimationFrameSetFactory.CreateEmpty()
                },
                {
                    PredefinedAnimationSid.Skill3,
                    AnimationFrameSetFactory.CreateEmpty()
                },
                {
                    PredefinedAnimationSid.Skill4,
                    AnimationFrameSetFactory.CreateEmpty()
                },
                {
                    PredefinedAnimationSid.Ult,
                    AnimationFrameSetFactory.CreateEmpty()
                },
                {
                    PredefinedAnimationSid.Wound,
                    AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 6 }, fps: 8)
                },
                {
                    PredefinedAnimationSid.Death,
                    AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 7 }, fps: 8)
                }
            };
        }
    }
}