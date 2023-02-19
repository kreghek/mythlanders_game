using Core.Dices;

namespace Core.Combats;

public interface ITargetSelectorContext
{
    CombatFieldSide ActorSide { get; }
    CombatFieldSide EnemySide { get; }
    IDice Dice { get; }
}