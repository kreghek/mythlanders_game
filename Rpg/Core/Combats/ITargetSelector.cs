namespace Core.Combats;

public interface ITargetSelector
{
    IReadOnlyList<Combatant> Get(Combatant actor, CombatField combatField);
}