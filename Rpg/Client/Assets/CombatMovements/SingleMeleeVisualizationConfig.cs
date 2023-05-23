using Client.GameScreens.Combat.GameObjects;

using Rpg.Client.Core;

namespace Client.Assets.CombatMovements;

internal sealed record SingleMeleeVisualizationConfig(
    IAnimationFrameSet PrepareMovementAnimation,
    IAnimationFrameSet CombatMovementAnimation,
    IAnimationFrameSet BackAnimation);