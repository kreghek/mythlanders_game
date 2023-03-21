namespace Core.Combats;

public interface ITargetSelector
{
    IReadOnlyList<CombatMoveTargetEstimate> GetEstimate(Combatant actor, ITargetSelectorContext context)
    {
        return GetMaterialized(actor, context)
            .Select(x => new CombatMoveTargetEstimate(x, CombatMoveTargetEstimateType.Exactly))
            .ToArray();
    }

    IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context);
}

public enum CombatMoveTargetEstimateType
{
    Exactly,
    Approximately
}

public sealed record CombatMoveTargetEstimate(Combatant Target, CombatMoveTargetEstimateType EstimateType);