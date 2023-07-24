namespace Core.Combats;

public record CombatEffectImposeItem(CombatEffectImposeDelegate ImposeDelegate,
    IReadOnlyList<Combatant> MaterializedTargets);