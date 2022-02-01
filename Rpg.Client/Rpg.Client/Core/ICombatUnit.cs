namespace Rpg.Client.Core
{
    internal interface ICombatUnit
    {
        Unit Unit { get; }

        void ChangeState(CombatUnitState targetState);
    }
}