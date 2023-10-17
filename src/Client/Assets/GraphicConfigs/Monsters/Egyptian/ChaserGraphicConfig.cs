using System.Collections.Generic;

using Client.Core;
using Client.Core.AnimationFrameSets;

using Microsoft.Xna.Framework;

namespace Client.Assets.GraphicConfigs.Monsters.Egyptian;

internal sealed class ChaserGraphicConfig : EgyptianMonsterGraphicConfig
{
    public ChaserGraphicConfig() : base(UnitName.Chaser)
    {
        ShadowOffset = new Vector2(70, 12);
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            {
                PredefinedAnimationSid.Idle,
                new LinearAnimationFrameSet(new[] { 0 }, 0, frameWidth: 128, frameHeight: 150, textureColumns: 3)
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
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 24, frameCount: 8, fps: 8)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateSequential(startFrameIndex: 32, frameCount: 8, fps: 8)
            }
        };
    }
}