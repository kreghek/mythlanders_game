using CombatDicesTeam.Combats;

namespace Core.Combats.TargetSelectors;

public sealed class MostShieldChargedEnemyTargetSelector : MostEnemyStatValueTargetSelectorBase, ITargetSelector
{
    public override IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        var enemies = context.EnemySide.GetAllCombatants().ToArray();

        if (enemies.Any())
        {
            return new[]
            {
                enemies.OrderByDescending(x => GetStatCurrentValue(x, CombatantStatTypes.ShieldPoints))
                    .First()
            };
        }

        return Array.Empty<ICombatant>();
    }
}