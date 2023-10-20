using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Client.Core.AnimationFrameSets;

internal class SoundedAnimationFrameSet : IAnimationFrameSet
{
    private readonly IAnimationFrameSet _baseFrameSet;

    public SoundedAnimationFrameSet(IAnimationFrameSet baseFrameSet, AudioSettings audioSettings,
        (int frameIndex, SoundEffect sound)[] sounds)
    {
        _baseFrameSet = baseFrameSet;

        _baseFrameSet.End += (_, args) => End?.Invoke(this, args);

        _baseFrameSet.KeyFrame += (_, args) =>
        {
            var soundToPlay = sounds.Where(x => x.frameIndex == args.FrameIndex);
            foreach (var tuple in soundToPlay)
            {
                var soundEffectInstance = tuple.sound.CreateInstance();
                soundEffectInstance.Volume = audioSettings.SfxVolume;
                soundEffectInstance.Play();
            }

            KeyFrame?.Invoke(this, args);
        };
    }

    public bool IsIdle => _baseFrameSet.IsIdle;
    public Rectangle GetFrameRect()
    {
        return _baseFrameSet.GetFrameRect();
    }

    public void Reset()
    {
        _baseFrameSet.Reset();
    }

    public void Update(GameTime gameTime)
    {
        _baseFrameSet.Update(gameTime);
    }

    public event EventHandler? End;
    public event EventHandler<KeyFrameEventArgs>? KeyFrame;
}