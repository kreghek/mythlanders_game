namespace Client.Assets.CombatMovements;

internal sealed record SingleMeleeVisualizationConfig(
    SoundedAnimation PrepareMovementAnimation,
    SoundedAnimation CombatMovementAnimation,
    SoundedAnimation HitAnimation,
    SoundedAnimation HitCompleteAnimation,
    SoundedAnimation BackAnimation);