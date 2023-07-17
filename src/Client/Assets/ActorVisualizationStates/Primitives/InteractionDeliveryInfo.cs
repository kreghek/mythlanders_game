using Core.Combats;

using Microsoft.Xna.Framework;

namespace Client.Assets.ActorVisualizationStates.Primitives;

internal sealed record InteractionDeliveryInfo(CombatEffectImposeItem ImposeItem, Vector2 StartPosition,
    Vector2 TargetPosition);