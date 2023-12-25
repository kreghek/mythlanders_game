using Microsoft.Xna.Framework;

namespace GameClient.Engine.Animations;

/// <summary>
/// Animation with sound effects.
/// </summary>
public sealed class SoundedAnimationFrameSet : IAnimationFrameSet
{
    private readonly IAnimationFrameSet _baseFrameSet;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="baseAnimation">Underlying animation</param>
    /// <param name="keySfx"> Sound effects bounded with frames. </param>
    public SoundedAnimationFrameSet(IAnimationFrameSet baseAnimation,
        IReadOnlyCollection<AnimationFrame<IAnimationSoundEffect>> keySfx)
    {
        _baseFrameSet = baseAnimation;

        _baseFrameSet.End += (_, args) => End?.Invoke(this, args);

        _baseFrameSet.KeyFrame += (_, args) =>
        {
            var soundToPlay = keySfx.Where(x => x.FrameInfo.Equals(args.KeyFrame));
            foreach (var animationSoundEffect in soundToPlay)
            {
                animationSoundEffect.Payload.Play();
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