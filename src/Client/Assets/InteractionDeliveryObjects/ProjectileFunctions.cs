using GameClient.Engine.MoveFunctions;

namespace Client.Assets.InteractionDeliveryObjects;

internal sealed record ProjectileFunctions(IMoveFunction MoveFunction, IRotationFunction RotationFunction);