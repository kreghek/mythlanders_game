using Core.Combats;

namespace Text.Client;

internal sealed class UseCombatMovementIntention : IIntention
{
    private readonly CombatMovementInstance _combatMovement;

    public UseCombatMovementIntention(CombatMovementInstance combatMovement)
    {
        _combatMovement = combatMovement;
    }

    public void Make(CombatCore combatCore)
    {
        var movementExecution = combatCore.CreateCombatMovementExecution(_combatMovement);
        PseudoPlayback(movementExecution);
        combatCore.CompleteTurn();
    }

    private static void PseudoPlayback(CombatMovementExecution movementExecution)
    {
        foreach (var imposeItem in movementExecution.EffectImposeItems)
            foreach (var target in imposeItem.MaterializedTargets)
                imposeItem.ImposeDelegate(target);

        movementExecution.CompleteDelegate();
    }
}