using GameClient.Engine.Animations;

using Microsoft.Xna.Framework.Audio;

namespace Client.Assets.CombatMovements;

public sealed class AudioPlayer: IAudioPlayer
{

    private const float SFX_VOLUME = 1;
    
    public void PlayEffect(SoundEffect soundEffect)
    {
        var soundEffectInstance = soundEffect.CreateInstance();
        soundEffectInstance.Volume = SFX_VOLUME;
        soundEffectInstance.Play();
    }
}