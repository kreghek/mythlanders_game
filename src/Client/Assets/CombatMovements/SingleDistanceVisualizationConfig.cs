using Client.Assets.ActorVisualizationStates.Primitives;

using GameClient.Engine.Animations;

namespace Client.Assets.CombatMovements;

internal sealed record SingleDistanceVisualizationConfig(
    IAnimationFrameSet PrepareAnimation,
    IAnimationFrameSet LaunchProjectileAnimation,
    IAnimationFrameSet WaitAnimation,
    IDeliveryFactory DeliveryFactory,
    IAnimationFrameInfo LaunchFrame);