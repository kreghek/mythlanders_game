using CombatDicesTeam.Combats;

namespace GameAssets.Combats.TargetSelectors;

internal class AttackerTargetSelector : ITargetSelector
{
    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        return context.GetCombatants(TargetSelectorContextCombatantTypes.Attacker).ToArray();
    }
}
