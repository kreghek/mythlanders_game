using System.Collections.Generic;

using Client.Core;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.GraphicConfigs.Heroes
{
    internal sealed class HopliteGraphicsConfig : UnitGraphicsConfigBase
    {
        public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
        {
            return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
            {
                {
                    PredefinedAnimationSid.Idle,
                    AnimationFrameSetFactory.CreateIdle(startFrameIndex: 0, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Defense,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 12, frameCount: 4, fps: 0)
                },
                {
                    PredefinedAnimationSid.MoveForward,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, fps: 6)
                },
                {
                    PredefinedAnimationSid.MoveBackward,
                    AnimationFrameSetFactory.CreateSequentialFromGrid(rows: new[] { 6 }, fps: 8)
                },

                {
                    PredefinedAnimationSid.Skill1,
                    AnimationFrameSetFactory.CreateSequentialFromGrid(rows: new[] { 4 }, fps: 8)
                },
                {
                    PredefinedAnimationSid.Skill2,
                    AnimationFrameSetFactory.CreateSequentialFromGrid(rows: new[] { 1 }, fps: 8)
                },
                {
                    PredefinedAnimationSid.Skill3,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 16, frameCount: 8, fps: 8)
                },
                {
                    PredefinedAnimationSid.Skill4,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, fps: 8)
                },

                {
                    PredefinedAnimationSid.Ult,
                    AnimationFrameSetFactory.CreateSequential(startFrameIndex: 59, frameCount: 2, fps: 8, isLoop: false)
                },

                {
                    PredefinedAnimationSid.Wound,
                    AnimationFrameSetFactory.CreateSequentialFromGrid(rows: new[] { 7 }, fps: 8)
                },
                {
                    PredefinedAnimationSid.Death,
                    AnimationFrameSetFactory.CreateSequentialFromGrid(rows: new[] { 8 }, fps: 8)
                }
            };
        }
    }
}