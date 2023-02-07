namespace Core.Combats;

public sealed class ClosestInLineTargetSelector : ITargetSelector
{
    public IReadOnlyList<Combatant> Get(Combatant actor, CombatField combatField)
    {
        var actorLine = 0;

        for (var columnIndex = 0; columnIndex < combatField.HeroSide.GetLength(0); columnIndex++)
        {
            for (var lineIndex = 0; lineIndex < combatField.HeroSide.GetLength(1); lineIndex++)
            {
                if (combatField.HeroSide[columnIndex, lineIndex].Combatant == actor)
                {
                    actorLine = lineIndex;
                }
            }
        }

        var closestEnemySlot = combatField.MonsterSide[0, actorLine];
        if (closestEnemySlot.Combatant is null)
        {
            closestEnemySlot = combatField.MonsterSide[1, actorLine];
        }

        if (closestEnemySlot.Combatant is null)
        {
            return ArraySegment<Combatant>.Empty;
        }

        return new[]
        {
            closestEnemySlot.Combatant
        };
    }
}