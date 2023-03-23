﻿namespace Core.Combats.TargetSelectors;

public sealed class MostShieldChargedEnemyTargetSelector : MostEnemyStatValueTargetSelectorBase, ITargetSelector
{
    public IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        var enemies = context.EnemySide.GetAllCombatants().ToArray();

        if (enemies.Any())
        {

            return new[]
            {
                enemies.OrderByDescending(x => GetStatCurrentValue(x, UnitStatType.ShieldPoints))
                .First()
            };
        }
        else
        {
            return Array.Empty<Combatant>();
        }
    }
}
