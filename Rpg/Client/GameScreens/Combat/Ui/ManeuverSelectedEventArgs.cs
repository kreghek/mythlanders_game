using System;

using Core.Combats;

namespace Client.GameScreens.Combat.Ui;

public sealed class ManeuverSelectedEventArgs : EventArgs
{
    public FieldCoords Coords { get; }

    public ManeuverSelectedEventArgs(FieldCoords coords)
    {
        Coords = coords;
    }
}