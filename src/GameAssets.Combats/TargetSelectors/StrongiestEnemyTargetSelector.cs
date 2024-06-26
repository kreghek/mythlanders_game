﻿using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Core.Combats.TargetSelectors;

public sealed class StrongestEnemyTargetSelector : MostEnemyStatValueTargetSelectorBase, ITargetSelector
{
    public override IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        var enemies = context.EnemySide.GetAllCombatants().ToArray();

        if (enemies.Any())
        {
            return new[]
            {
                enemies.OrderByDescending(x => GetStatCurrentValue(x, CombatantStatTypes.HitPoints))
                    .First()
            };
        }

        return Array.Empty<ICombatant>();
    }
}