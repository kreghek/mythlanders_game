namespace Core.Combats;

public sealed class CombatantDamagedEventArgs : EventArgs
{
    public CombatantDamagedEventArgs(Combatant combatant, UnitStatType statType, int value)
    {
        Combatant = combatant;
        StatType = statType;
        Value = value;
    }

    public Combatant Combatant { get; }
    public UnitStatType StatType { get; }
    public int Value { get; }
}