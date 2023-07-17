using System.Collections.Generic;

using Client.Core;

using Microsoft.Xna.Framework;

namespace Client.Assets.GraphicConfigs.Monsters;

internal sealed class DigitalWolfGraphicsConfig : SlavicMonsterGraphicConfig
{
    public DigitalWolfGraphicsConfig(UnitName unit) : base(unit)
    {
        InteractionPoint = new Vector2(64, 16);
        StatsPanelOrigin = new Vector2(32, 64 - 10);
        ShadowOrigin = new Vector2(16, 0);
    }

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