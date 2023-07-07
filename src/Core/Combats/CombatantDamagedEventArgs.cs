namespace Core.Combats;

public sealed class CombatantDamagedEventArgs : EventArgs
{
    public CombatantDamagedEventArgs(Combatant combatant, ICombatantStatType statType, int value)
    {
        Combatant = combatant;
        StatType = statType;
        Value = value;
    }

    public Combatant Combatant { get; }
    public ICombatantStatType StatType { get; }
    public int Value { get; }
}