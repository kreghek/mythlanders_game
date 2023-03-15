namespace Core.Combats;

public sealed class CombatantHasBeenMovedEventArgs : CombatantEventArgsBase
{
    public CombatantHasBeenMovedEventArgs(Combatant combatant, CombatFieldSide fieldSide, FieldCoords newFieldCoords) :
        base(combatant)
    {
        FieldSide = fieldSide;
        NewFieldCoords = newFieldCoords;
    }

    public CombatFieldSide FieldSide { get; }
    public FieldCoords NewFieldCoords { get; }
}