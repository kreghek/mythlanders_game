using CombatDicesTeam.Combats;

namespace Core.Combats.BotBehaviour;

/// <summary>
/// Factory to generate client-specific intetions.
/// </summary>
public interface IIntentionFactory
{
    /// <summary>
    /// Create intention to use specified combat movement.
    /// </summary>
    /// <param name="combatMovement">Combat movement to use.</param>
    IIntention CreateCombatMovement(CombatMovementInstance combatMovement);
}