namespace Core.Combats;

public sealed class CombatantHasBeenMovedEventArgs : CombatantEventArgsBase
{
    public CombatFieldSide FieldSide { get; }
    public FieldCoords NewFieldCoords { get; }

    public CombatantHasBeenMovedEventArgs(Combatant combatant, CombatFieldSide fieldSide, FieldCoords newFieldCoords) : base(combatant)
    {
        FieldSide = fieldSide;
        NewFieldCoords = newFieldCoords;
    }
}