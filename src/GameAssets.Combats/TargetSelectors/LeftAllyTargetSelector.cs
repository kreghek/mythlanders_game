using CombatDicesTeam.Combats;

namespace Core.Combats.TargetSelectors;

public sealed class LeftAllyTargetSelector : ITargetSelector
{
    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        var target = GetLeftAlly(actor, context.ActorSide);
        if (target is null)
        { 
            return Array.Empty<ICombatant>();
        }

        return new[] { target };
    }

    private static ICombatant? GetLeftAlly(ICombatant baseCombatant, CombatFieldSide side)
    {
        var currentCoords = side.GetCombatantCoords(baseCombatant);

        if (currentCoords.LineIndex == 0)
        {
            return null;
        }

        var rightCoords = new FieldCoords(currentCoords.ColumentIndex, currentCoords.LineIndex - 1);

        return side[rightCoords].Combatant;
    }
}