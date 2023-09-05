using CombatDicesTeam.Combats;
using CombatDicesTeam.Dices;

namespace Core.Combats.TargetSelectors;

public sealed class RandomLineAllyTargetSelector : ITargetSelector
{
    private IEnumerable<ICombatant> GetIterator(ITargetSelectorContext context)
    {
        var fieldSide = context.ActorSide;

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

    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        var enemies = GetIterator(context).ToArray();
        var target = context.Dice.RollFromList(enemies);
        return new[]
        {
            target
        };
    }
}