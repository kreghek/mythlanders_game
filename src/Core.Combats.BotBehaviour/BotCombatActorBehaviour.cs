using CombatDicesTeam.Combats;

namespace Core.Combats.BotBehaviour;

/// <summary>
/// Bot behaviour for combat.
/// </summary>
public sealed class BotCombatActorBehaviour : ICombatActorBehaviour
{
    private readonly IIntentionFactory _intentionFactory;

    public BotCombatActorBehaviour(IIntentionFactory intentionFactory)
    {
        _intentionFactory = intentionFactory;
    }

    /// <inheritdoc />
    public void HandleIntention(ICombatantBehaviourData combatData, Action<IIntention> intentionDelegate)
    {
        var firstCombatMove = combatData.CurrentActor.CombatMoves.First();

        var combatMoveIntention = _intentionFactory.CreateCombatMovement(firstCombatMove.CombatMovement);

        intentionDelegate(combatMoveIntention);
    }
}