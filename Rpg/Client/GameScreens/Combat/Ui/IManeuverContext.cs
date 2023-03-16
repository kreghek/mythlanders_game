using Core.Combats;

namespace Client.GameScreens.Combat.Ui;

internal interface IManeuverContext
{
    int? ManeuversAvailable { get; }
    FieldCoords? ManeuverStartCoords { get; }

    CombatFieldSide FieldSide { get; }
}