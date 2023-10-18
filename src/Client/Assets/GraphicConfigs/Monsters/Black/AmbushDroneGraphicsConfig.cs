using System.Collections.Generic;
using System.Linq;

using Client.Core;

using Microsoft.Xna.Framework;

namespace Client.Assets.GraphicConfigs.Monsters.Black;

internal sealed class AmbushDroneGraphicsConfig : BlackMonsterGraphicConfig
{
    public AmbushDroneGraphicsConfig(UnitName unit) : base(unit)
    {
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            {
                PredefinedAnimationSid.Idle,
                AnimationFrameSetFactory.CreateIdle(fps: 8, frameCount: 4, textureColumns: 4, frameWidth: 128)
            },
            {
                PredefinedAnimationSid.MoveForward,
                AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 0 }, fps: 8, textureColumns: 4,
                    frameWidth: 128)
            },
            {
                PredefinedAnimationSid.MoveBackward,
                AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 0 }, fps: 8, textureColumns: 4,
                    frameWidth: 128)
            },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 0 }, fps: 8, textureColumns: 4,
                    frameWidth: 128)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateSequentialFromGrid(Enumerable.Range(1, 4).ToArray(), fps: 32,
                    textureColumns: 4, frameWidth: 128)
            }
        };
    }
}