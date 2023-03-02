using Core.Combats;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects;

internal interface IUnitPositionProvider
{
    Vector2 GetPosition(FieldCoords formationCoords, bool isPlayerSide);
}