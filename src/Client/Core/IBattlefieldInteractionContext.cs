using CombatDicesTeam.Combats;

using Microsoft.Xna.Framework;

namespace Client.Core;

internal interface IBattlefieldInteractionContext
{
    public Rectangle GetArea(Team side);
    public Vector2 GetCombatantPosition(ICombatant combatant);
}