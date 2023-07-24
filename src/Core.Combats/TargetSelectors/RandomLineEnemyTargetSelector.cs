using Core.Dices;

namespace Core.Combats.TargetSelectors;

public sealed class RandomLineEnemyTargetSelector : ITargetSelector
{
    private IEnumerable<Combatant> GetIterator(ITargetSelectorContext context)
    {
        for (var lineIndex = 0; lineIndex < context.EnemySide.LineCount; lineIndex++)
        {
            var slot = context.EnemySide[new FieldCoords(0, lineIndex)];
            if (slot.Combatant is not null)
            {
                yield return slot.Combatant;
            }
        }
    }

    public IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        var enemies = GetIterator(context).ToArray();
        var target = context.Dice.RollFromList(enemies);
        return new[]
        {
            target
        };
    }
}