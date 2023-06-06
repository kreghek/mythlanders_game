namespace Core.Combats;

public sealed class CombatantHasBeenAddedEventArgs : CombatantEventArgsBase
{
    public CombatantHasBeenAddedEventArgs(Combatant combatant, CombatFieldInfo fieldInfo) : base(combatant)
    {
        FieldInfo = fieldInfo;
    }

    public CombatFieldInfo FieldInfo { get; }
}