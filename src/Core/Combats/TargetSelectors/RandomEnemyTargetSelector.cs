using Core.Dices;

namespace Core.Combats.TargetSelectors;

public sealed class RandomEnemyTargetSelector : ITargetSelector
{
    private IEnumerable<Combatant> GetIterator(ITargetSelectorContext context)
    {
        var fieldSide = context.EnemySide;

        for (var lineIndex = 0; lineIndex < fieldSide.LineCount; lineIndex++)
            for (var columnIndex = 0; columnIndex < fieldSide.ColumnCount; columnIndex++)
            {
                var slot = fieldSide[new FieldCoords(columnIndex, lineIndex)];
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