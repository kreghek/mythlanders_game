namespace Core.Combats;

public sealed record TargetSelectorContext(CombatFieldSide ActorSide, CombatFieldSide EnemySide) : ITargetSelectorContext;