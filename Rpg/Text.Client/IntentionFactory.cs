using Core.Combats;
using Core.Combats.BotBehaviour;

namespace Text.Client;

internal sealed class IntentionFactory : IIntentionFactory
{
    public IIntention CreateCombatMovement(CombatMovementInstance combatMovement)
    {
        return new UseCombatMovementIntention(combatMovement);
    }
}