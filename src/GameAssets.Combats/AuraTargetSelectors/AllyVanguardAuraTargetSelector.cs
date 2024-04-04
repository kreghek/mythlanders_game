using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace GameAssets.Combats.AuraTargetSelectors;

public sealed class AllyVanguardAuraTargetSelector : IAuraTargetSelector
{
    private static IEnumerable<ICombatant> GetIterator(CombatFieldSide side)
    {
        for (var lineIndex = 0; lineIndex < side.LineCount; lineIndex++)
        {
            var slot = side[new FieldCoords(0, lineIndex)];
            if (slot.Combatant is not null)
            {
                yield return slot.Combatant;
            }
        }
    }

    private static CombatFieldSide GetTargetSide(ICombatant target, CombatField field)
    {
        var heroes = field.HeroSide.GetAllCombatants();
        if (heroes.Contains(target))
        {
            return field.HeroSide;
        }

        return field.MonsterSide;
    }

    private static IEnumerable<ICombatant> GetVanguardCombatant(CombatFieldSide side)
    {
        return GetIterator(side).ToArray();
    }

    private static bool IsInVanguard(ICombatant testCombatant, IAuraTargetSelectorContext context)
    {
        var testCombatantSide = GetTargetSide(testCombatant, context.Combat.Field);

        var vanguards = GetVanguardCombatant(testCombatantSide);

        return vanguards.Contains(testCombatant);
    }

    public bool IsCombatantUnderAura(ICombatant auraOwner, ICombatant testCombatant,
        IAuraTargetSelectorContext context)
    {
        return auraOwner.IsPlayerControlled == testCombatant.IsPlayerControlled &&
               IsInVanguard(testCombatant, context);
    }
}