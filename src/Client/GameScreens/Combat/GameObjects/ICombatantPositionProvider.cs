using Core.Combats;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects;

internal interface ICombatantPositionProvider
{
    Vector2 GetPosition(FieldCoords formationCoords, CombatantPositionSide side);
}