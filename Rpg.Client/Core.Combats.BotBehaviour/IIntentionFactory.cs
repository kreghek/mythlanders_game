namespace Core.Combats.BotBehaviour;

public interface IIntentionFactory
{
    IIntention CreateCombatMovement(CombatMovementInstance combatMovement);
}