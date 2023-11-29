using System;

using Client.Core;
using Client.Engine;

using GameClient.Engine.Animations;
using GameClient.Engine.CombatVisualEffects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Assets.CombatVisualEffects;

internal sealed class EnergyBlastVisualEffect : ICombatVisualEffect
{
    private const float FPS = 8 * 2f;
    private readonly IAnimationFrameSet _frameSet;

    private readonly Sprite _graphics;
    private readonly ParticleSystem _tailParticleSystem;

    public EnergyBlastVisualEffect(Vector2 position, Texture2D blastTexture, Texture2D particlesTexture)
    {
        _frameSet = AnimationFrameSetFactory.CreateSequential(7, frameCount: 7, fps: FPS, frameWidth: 64,
            frameHeight: 32, textureColumns: SfxSpriteConsts.Size64x32.COL_COUNT, isLoop: false);

        _graphics = new Sprite(blastTexture)
        {
            Position = position,
            SourceRectangle = Rectangle.Empty
        };

        _frameSet.End += FrameSet_End;

        var particleGenerator = new TailParticleGenerator(new[] { particlesTexture });
        _tailParticleSystem = new ParticleSystem(position, particleGenerator);
    }

    private void DrawForegroundAdditionalEffects(SpriteBatch spriteBatch)
    {
        _tailParticleSystem.Draw(spriteBatch);
    }

    private void FrameSet_End(object? sender, EventArgs e)
    {
        IsDestroyed = true;
    }

    private void UpdateAdditionalEffects(GameTime gameTime)
    {
        _tailParticleSystem.Update(gameTime);
    }

    public bool IsDestroyed { get; private set; }

    public void DrawBack(SpriteBatch spriteBatch)
    {
        // Draw nothing
    }

    public void DrawFront(SpriteBatch spriteBatch)
    {
        if (IsDestroyed)
        {
            return;
        }

        _graphics.Draw(spriteBatch);

        DrawForegroundAdditionalEffects(spriteBatch);
    }

    public void Update(GameTime gameTime)
    {
        if (IsDestroyed)
        {
            return;
        }

        _frameSet.Update(gameTime);
        _graphics.SourceRectangle = _frameSet.GetFrameRect();

        UpdateAdditionalEffects(gameTime);
    }
}