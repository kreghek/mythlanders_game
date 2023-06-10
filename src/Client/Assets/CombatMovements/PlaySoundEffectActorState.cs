using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Client.Assets.CombatMovements;

internal sealed class PlaySoundEffectActorState : IActorVisualizationState
{
    private readonly SoundEffectInstance _soundEffect;

    private bool _soundPlayed;

    public PlaySoundEffectActorState(SoundEffectInstance soundEffect)
    {
        _soundEffect = soundEffect;
    }

    public bool CanBeReplaced => true;

    public bool IsComplete => _soundEffect.State == SoundState.Stopped;

    public void Cancel()
    {
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