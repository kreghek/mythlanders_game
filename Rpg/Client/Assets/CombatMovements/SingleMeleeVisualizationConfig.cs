using Rpg.Client.Core;

namespace Client.Assets.CombatMovements;

internal sealed record SingleMeleeVisualizationConfig(IAnimationFrameSet CombatMovementAnimation,
    IAnimationFrameSet BackAnimation);