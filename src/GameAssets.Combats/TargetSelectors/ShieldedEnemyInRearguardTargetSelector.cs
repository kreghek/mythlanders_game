using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Core.Combats.TargetSelectors;

public sealed class ShieldedEnemyInRearguardTargetSelector : MostEnemyStatValueTargetSelectorBase, ITargetSelector
{
    private static IEnumerable<ICombatant> GetRearguardIterator(ITargetSelectorContext context)
    {
        for (var lineIndex = 0; lineIndex < context.EnemySide.LineCount; lineIndex++)
        {
            var slot = context.EnemySide[new FieldCoords(1, lineIndex)];
            if (slot.Combatant is not null)
            {
                yield return slot.Combatant;
            }
        }
    }

    public override IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        var rearguardEnemies = GetRearguardIterator(context).ToArray();

        if (rearguardEnemies.Any())
        {
            return new[]
            {
                rearguardEnemies.OrderBy(x => GetStatCurrentValue(x, CombatantStatTypes.ShieldPoints))
                    .First()
            };
        }

        return Array.Empty<ICombatant>();
    }
}