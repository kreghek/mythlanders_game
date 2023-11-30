using System;
using System.Collections.Generic;
using System.Linq;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;

namespace Client.Engine.PostProcessing;

/// <summary>
/// Animation with post-processing visual effects.
/// </summary>
public sealed class PostEffectAnimationFrameSet : IAnimationFrameSet
{
    private readonly IAnimationFrameSet _baseFrameSet;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="baseAnimation">Underlying animation</param>
    /// <param name="postEffectManager"> Post-processing effect manager to play effect. </param>
    /// <param name="keySfx"> Post-processing effects bounded with frames. </param>
    public PostEffectAnimationFrameSet(IAnimationFrameSet baseAnimation,
        PostEffectManager postEffectManager,
        IReadOnlyCollection<AnimationFrame<IPostEffect>> keySfx)
    {
        _baseFrameSet = baseAnimation;

        _baseFrameSet.End += (_, args) =>
        {
            foreach (var effect in keySfx)
            {
                postEffectManager.RemoveEffect(effect.Payload);
            }

            End?.Invoke(this, args);
        };

        _baseFrameSet.KeyFrame += (_, args) =>
        {
            var effectsToPlay = keySfx.Where(x => x.FrameInfo.Equals(args.KeyFrame));
            foreach (var effect in effectsToPlay)
            {
                postEffectManager.AddEffect(effect.Payload);
            }

            KeyFrame?.Invoke(this, args);
        };
    }

    /// <inheritdoc />
    public bool IsIdle => _baseFrameSet.IsIdle;

    /// <inheritdoc />
    public Rectangle GetFrameRect()
    {
        return _baseFrameSet.GetFrameRect();
    }

    /// <inheritdoc />
    public void Reset()
    {
        _baseFrameSet.Reset();
    }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        _baseFrameSet.Update(gameTime);
    }

    /// <inheritdoc />
    public event EventHandler? End;

    /// <inheritdoc />
    public event EventHandler<AnimationFrameEventArgs>? KeyFrame;
}