using CombatDicesTeam.Combats;

using GameAssets.Combats.CombatantStatuses;

namespace GameAssets.Combats.EffectConditions;
public sealed class IsRightAllyWithShieldCondition : IEffectCondition
{
    public bool Check(ICombatant actor, CombatField combatField)
    {
        var rightAllyCombatant = GetRightAlly(actor, combatField);

        if (rightAllyCombatant is null)
        {
            return false;
        }

        var isRightCombatantHasShield = CheckIsCombatantHasShield(rightAllyCombatant);

        return isRightCombatantHasShield;
    }

    private static bool CheckIsCombatantHasShield(ICombatant testCombatant)
    {
        return testCombatant.Statuses.Any(x => ReferenceEquals(x, SystemStatuses.HasShield));
    }

    private static ICombatant? GetRightAlly(ICombatant baseCombatant, CombatField field)
    {
        var side = GetTargetSide(baseCombatant, field);
        var currentCoords = side.GetCombatantCoords(baseCombatant);

        if (currentCoords.LineIndex == 2)
        {
            return null;
        }

        var rightCoords = new FieldCoords(currentCoords.ColumentIndex, currentCoords.LineIndex + 1);

        return side[rightCoords].Combatant;
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
}
