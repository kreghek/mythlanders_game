namespace Core.Combats;

public abstract class CombatantEventArgsBase : EventArgs
{
    protected CombatantEventArgsBase(Combatant combatant)
    {
        Combatant = combatant;
    }

    public Combatant Combatant { get; }
}

public sealed class CombatantDefeatedEventArgs : CombatantEventArgsBase
{
    public CombatantDefeatedEventArgs(Combatant combatant) : base(combatant)
    {
    }
}

public sealed class CombatantEndsTurnEventArgs : CombatantEventArgsBase
{
    public CombatantEndsTurnEventArgs(Combatant combatant) : base(combatant)
    {
    }
}

public sealed class CombatantTurnStartedEventArgs : CombatantEventArgsBase
{
    public CombatantTurnStartedEventArgs(Combatant combatant) : base(combatant)
    {
    }
}

public sealed record CombatFieldInfo(CombatFieldSide FieldSide, FieldCoords CombatantCoords);

public sealed class CombatantHasBeenAddedEventArgs : CombatantEventArgsBase
{
    public CombatFieldInfo FieldInfo { get; }

    public CombatantHasBeenAddedEventArgs(Combatant combatant, CombatFieldInfo fieldInfo) : base(combatant)
    {
        FieldInfo = fieldInfo;
    }
}

public sealed class CombatantHasBeenRemovedEventArgs : CombatantEventArgsBase
{
    public CombatantHasBeenRemovedEventArgs(Combatant combatant) : base(combatant)
    {
    }
}


public sealed class CombatantShiftShapedEventArgs: CombatantEventArgsBase
{
    public CombatantShiftShapedEventArgs(Combatant combatant) : base(combatant)
    {
    }
}

public sealed class CombatantHasBeenMovedEventArgs: CombatantEventArgsBase
{
    public CombatantHasBeenMovedEventArgs(Combatant combatant) : base(combatant)
    {
    }
}