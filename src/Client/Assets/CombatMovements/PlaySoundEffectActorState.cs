using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Client.Assets.CombatMovements;

internal sealed class PlaySoundEffectActorState : IActorVisualizationState
{
    public bool CanBeReplaced => true;

    public bool IsComplete => _soundEffect.State == SoundState.Stopped;

    public void Cancel()
    {
    }

    private bool _soundPlayed;
    private readonly SoundEffectInstance _soundEffect;

    public PlaySoundEffectActorState(SoundEffectInstance soundEffect)
    {
        _soundEffect = soundEffect;
    }

    public void Update(GameTime gameTime)
    {
        if (!_soundPlayed)
        {
            _soundEffect.Play();
            _soundPlayed = true;
        }
    }
}
