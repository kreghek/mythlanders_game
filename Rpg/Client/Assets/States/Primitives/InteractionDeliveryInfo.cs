using Core.Combats;

using Microsoft.Xna.Framework;

namespace Client.Assets.States.Primitives;

internal sealed record InteractionDeliveryInfo(CombatEffectImposeItem ImposeItem, Vector2 TargetPosition);