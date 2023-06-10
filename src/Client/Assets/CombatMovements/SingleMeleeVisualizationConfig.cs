using Client.Core;

namespace Client.Assets.CombatMovements;

internal sealed record SingleMeleeVisualizationConfig(
    SoundedAnimation PrepareMovementAnimation,
    SoundedAnimation CombatMovementAnimation,
    SoundedAnimation HitAnimation,
    IAnimationFrameSet HitCompleteAnimation,
    IAnimationFrameSet BackAnimation)
{
    public SingleMeleeVisualizationConfig(IAnimationFrameSet prepareMovementAnimation,
    IAnimationFrameSet combatMovementAnimation,
    IAnimationFrameSet hitAnimation,
    IAnimationFrameSet hitCompleteAnimation,
    IAnimationFrameSet backAnimation) : this(
        new SoundedAnimation(prepareMovementAnimation, null),
        new SoundedAnimation(combatMovementAnimation, null),
        new SoundedAnimation(hitAnimation, null),
        hitCompleteAnimation,
        backAnimation)
    { }
}