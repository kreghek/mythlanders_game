
using Microsoft.Xna.Framework.Audio;

namespace Rpg.Client.GameScreens.Event.Ui
{
    internal sealed class SpeechSoundWrapper: ISpeechSoundWrapper
    {
        private readonly SoundEffect _soundEffect;
        private SoundEffectInstance? _currentSound;

        public SpeechSoundWrapper(SoundEffect soundEffect)
        {
            _soundEffect = soundEffect;
        }

        public double Duration => _soundEffect.Duration.TotalSeconds;
        public void Play()
        {
            if (_currentSound is not null && _currentSound.State != SoundState.Stopped)
            {
                return;
            }

            _currentSound = _soundEffect.CreateInstance();
            _currentSound.Volume = 0.5f;
            _currentSound.Play();
        }
    }
}