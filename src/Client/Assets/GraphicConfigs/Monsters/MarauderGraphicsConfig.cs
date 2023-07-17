using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.GraphicConfigs.Monsters;

internal sealed class MarauderGraphicsConfig : BlackMonsterGraphicConfig
{
    public MarauderGraphicsConfig(UnitName unit) : base(unit)
    {
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdle(fps: 5) },
            {
                PredefinedAnimationSid.MoveForward,
                AnimationFrameSetFactory.CreateEmpty()
            },
            {
                PredefinedAnimationSid.MoveBackward,
                AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 2 }, fps: 8)
            },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 3 }, fps: 8)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 4 }, fps: 8)
            }
        };
    }
}