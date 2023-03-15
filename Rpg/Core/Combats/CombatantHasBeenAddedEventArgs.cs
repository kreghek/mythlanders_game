namespace Core.Combats;

public sealed class CombatantHasBeenAddedEventArgs : CombatantEventArgsBase
{
    public CombatFieldInfo FieldInfo { get; }

    public CombatantHasBeenAddedEventArgs(Combatant combatant, CombatFieldInfo fieldInfo) : base(combatant)
    {
        FieldInfo = fieldInfo;
    }
}
