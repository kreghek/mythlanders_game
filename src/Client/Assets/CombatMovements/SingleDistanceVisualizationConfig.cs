using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Core;

using GameClient.Engine.Animations;

namespace Client.Assets.CombatMovements;

internal sealed record SingleDistanceVisualizationConfig(
    IAnimationFrameSet LaunchAnimation,
    IAnimationFrameSet LookOnProjectileAnimation,
    IDeliveryFactory DeliveryFactory);