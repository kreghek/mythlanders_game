using System;

using Core.Combats;

namespace Client.GameScreens.Combat.Ui;

public sealed class ManeuverSelectedEventArgs : EventArgs
{
    public ManeuverSelectedEventArgs(FieldCoords coords)
    {
        Coords = coords;
    }

    public FieldCoords Coords { get; }
}