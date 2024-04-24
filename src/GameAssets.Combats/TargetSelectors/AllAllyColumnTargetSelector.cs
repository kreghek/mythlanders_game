using CombatDicesTeam.Combats;

namespace Core.Combats.TargetSelectors;

public sealed class AllAllyColumnTargetSelector : ITargetSelector
{
    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        var actorCoords = context.ActorSide.GetCombatantCoords(actor);

        var allyInColumn = context.ActorSide.GetColumnCombatants(actorCoords.ColumentIndex);

        return allyInColumn.ToArray();
    }
}