using Core.Dices;

namespace Core.Combats.TargetSelectors;

public sealed class RandomAllyTargetSelector : ITargetSelector
{
    private IEnumerable<Combatant> GetIterator(ITargetSelectorContext context)
    {
        for (var lineIndex = 0; lineIndex < context.EnemySide.LineCount; lineIndex++)
        {
            var slot = context.ActorSide[new FieldCoords(0, lineIndex)];
            if (slot.Combatant is not null) yield return slot.Combatant;
        }
    }

    public IReadOnlyList<Combatant> Get(Combatant actor, ITargetSelectorContext context)
    {
        var enemies = GetIterator(context).ToArray();
        var target = context.Dice.RollFromList(enemies);
        return new[]
        {
            target
        };
    }
}

public sealed class RandomLineAllyTargetSelector : ITargetSelector
{
    private IEnumerable<Combatant> GetIterator(ITargetSelectorContext context)
    {
        var fieldSide = context.ActorSide;

        for (var lineIndex = 0; lineIndex < fieldSide.LineCount; lineIndex++)
        for (var columnIndex = 0; columnIndex < fieldSide.ColumnCount; columnIndex++)
        {
            var slot = fieldSide[new FieldCoords(columnIndex, lineIndex)];
            if (slot.Combatant is not null) yield return slot.Combatant;
        }
    }

    public IReadOnlyList<Combatant> Get(Combatant actor, ITargetSelectorContext context)
    {
        var enemies = GetIterator(context).ToArray();
        var target = context.Dice.RollFromList(enemies);
        return new[]
        {
            target
        };
    }
}