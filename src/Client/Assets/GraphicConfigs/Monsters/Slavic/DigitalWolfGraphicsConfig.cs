using System.Collections.Generic;

using Client.Core;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;

namespace Client.Assets.GraphicConfigs.Monsters.Slavic;

internal sealed class DigitalWolfGraphicsConfig : SlavicMonsterGraphicConfig
{
    public DigitalWolfGraphicsConfig(UnitName unit) : base(unit)
    {
        InteractionPoint = new Vector2(64, 16);

        Origin = new Vector2(100, 106);
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            { PredefinedAnimationSid.Idle, AnimationFrameSetFactory.CreateIdleFromGrid(rows: new[] { 0, 1, 2 }) },
            {
                PredefinedAnimationSid.MoveForward,
                AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 5 }, fps: 6)
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