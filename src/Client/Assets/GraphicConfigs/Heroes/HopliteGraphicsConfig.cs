using System.Collections.Generic;

using Client.Core;

using GameClient.Engine.Animations;

namespace Client.Assets.GraphicConfigs.Heroes;

internal sealed class HopliteGraphicsConfig : HeroGraphicConfig
{
    public HopliteGraphicsConfig(UnitName name) : base(name)
    {
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            {
                PredefinedAnimationSid.Idle,
                AnimationFrameSetFactory.CreateIdle(startFrameIndex: 0, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.DefenseStance,
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