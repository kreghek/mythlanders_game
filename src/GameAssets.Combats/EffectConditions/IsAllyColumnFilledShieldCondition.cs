using CombatDicesTeam.Combats;

namespace GameAssets.Combats.EffectConditions;

public sealed class IsAllyColumnFilledShieldCondition : IEffectCondition
{
    private static CombatFieldSide GetTargetSide(ICombatant target, CombatField field)
    {
        var heroes = field.HeroSide.GetAllCombatants();
        if (heroes.Contains(target))
        {
            return field.HeroSide;
        }

        return field.MonsterSide;
    }

    public bool Check(ICombatant actor, CombatField combatField)
    {
        var side = GetTargetSide(actor, combatField);
        var currentCoords = side.GetCombatantCoords(actor);

        for (var i = 0; i < currentCoords.LineIndex; i++)
        {
            if (side[new FieldCoords(currentCoords.ColumentIndex, 0)].Combatant is null)
            {
                return false;
            }
        }

        return true;
    }
}