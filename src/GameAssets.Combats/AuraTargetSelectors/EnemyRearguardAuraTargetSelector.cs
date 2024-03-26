using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace GameAssets.Combats.AuraTargetSelectors;

public sealed class EnemyRearguardAuraTargetSelector : IAuraTargetSelector
{
    private static IEnumerable<ICombatant> GetIterator(CombatFieldSide side)
    {
        for (var lineIndex = 0; lineIndex < side.LineCount; lineIndex++)
        {
            var slot = side[new FieldCoords(1, lineIndex)];
            if (slot.Combatant is not null)
            {
                yield return slot.Combatant;
            }
        }
    }

    private static IEnumerable<ICombatant> GetRearguardCombatant(CombatFieldSide side)
    {
        return GetIterator(side).ToArray();
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

    private static bool IsInRearguard(ICombatant testCombatant, IAuraTargetSelectorContext context)
    {
        var testCombatantSide = GetTargetSide(testCombatant, context.Combat.Field);

        var vanguards = GetRearguardCombatant(testCombatantSide);

        return vanguards.Contains(testCombatant);
    }

    public bool IsCombatantUnderAura(ICombatant auraOwner, ICombatant testCombatant,
        IAuraTargetSelectorContext context)
    {
        return auraOwner.IsPlayerControlled != testCombatant.IsPlayerControlled &&
               IsInRearguard(testCombatant, context);
    }
}