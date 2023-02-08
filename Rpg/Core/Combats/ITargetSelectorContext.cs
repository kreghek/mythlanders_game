namespace Core.Combats;

public interface ITargetSelectorContext
{ 
    CombatFieldSide ActorSide { get; }
    CombatFieldSide EnemySide { get; }
}