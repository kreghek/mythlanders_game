using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Core;

namespace Client.Assets.CombatMovements;

internal sealed record SingleDistanceVisualizationConfig(
    IAnimationFrameSet LaunchAnimation,
    IAnimationFrameSet LookOnProjectileAnimation,
    IDeliveryFactory DeliveryFactory);