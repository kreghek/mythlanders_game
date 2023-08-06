using CombatDicesTeam.Combats;

namespace Core.Combats.TargetSelectors;

public sealed class AllAllyTargetSelector : ITargetSelector
{
    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        return context.ActorSide.GetAllCombatants().ToArray();
    }
}