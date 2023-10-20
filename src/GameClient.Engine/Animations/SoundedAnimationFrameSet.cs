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
    /// <param name="audioPlayer"></param>
    /// <param name="sounds"></param>
    public SoundedAnimationFrameSet(IAnimationFrameSet baseAnimation, IAudioPlayer audioPlayer,
        IReadOnlyCollection<AnimationSoundEffect> sounds)
    {
        _baseFrameSet = baseAnimation;

        _baseFrameSet.End += (_, args) => End?.Invoke(this, args);

        _baseFrameSet.KeyFrame += (_, args) =>
        {
            var soundToPlay = sounds.Where(x => x.PlayFrameIndex == args.FrameIndex);
            foreach (var animationSoundEffect in soundToPlay)
            {
                audioPlayer.PlayEffect(animationSoundEffect.SoundEffect);
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
    public event EventHandler<KeyFrameEventArgs>? KeyFrame;
}