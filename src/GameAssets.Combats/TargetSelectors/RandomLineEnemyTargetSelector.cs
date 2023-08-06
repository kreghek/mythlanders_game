using CombatDicesTeam.Combats;
using CombatDicesTeam.Dices;

namespace Core.Combats.TargetSelectors;

public sealed class RandomLineEnemyTargetSelector : ITargetSelector
{
    private IEnumerable<ICombatant> GetIterator(ITargetSelectorContext context)
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