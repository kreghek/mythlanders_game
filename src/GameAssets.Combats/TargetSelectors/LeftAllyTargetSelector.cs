using CombatDicesTeam.Combats;

namespace Core.Combats.TargetSelectors;

public sealed class LeftAllyTargetSelector : ITargetSelector
{
    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        throw new NotImplementedException();
    }
}