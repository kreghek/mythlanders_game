using GameClient.Engine.Animations;

using Microsoft.Xna.Framework.Audio;

namespace Client.Assets.CombatMovements;

public sealed class AnimationSoundEffect : IAnimationSoundEffect
{
    private readonly SoundEffect _soundEffect;
    private readonly AudioSettings _audioSettings;

    public AnimationSoundEffect(SoundEffect soundEffect, AudioSettings audioSettings)
    {
        _soundEffect = soundEffect;
        _audioSettings = audioSettings;
    }
    
    
    public void Play()
    {
        var soundEffectInstance = _soundEffect.CreateInstance();
        soundEffectInstance.Volume = _audioSettings.SfxVolume;
        soundEffectInstance.Play();
    }
}
