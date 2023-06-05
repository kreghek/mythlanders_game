using Client.Engine;

namespace Client.Assets.InteractionDeliveryObjects;

internal sealed record ProjectileFunctions(IMoveFunction MoveFunction, IRotationFunction RotationFunction);