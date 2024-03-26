using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace GameAssets.Combats.AuraTargetSelectors;

public class AllyVanguardAuraTargetSelector: IAuraTargetSelector
{
    public bool IsCombatantUnderAura(ICombatant auraOwner, ICombatant testCombatant)
    {
        return auraOwner.IsPlayerControlled == testCombatant.IsPlayerControlled && IsCombatantInVanguard(testCombatant);
    }

    private bool IsCombatantInVanguard(ICombatant testCombatant)
    {
        throw new NotImplementedException();
    }
}