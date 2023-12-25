using CombatDicesTeam.Combats;

namespace Client.GameScreens.Combat.Ui;

internal interface IManeuverContext
{
    CombatFieldSide FieldSide { get; }
    int? ManeuversAvailableCount { get; }
    FieldCoords? ManeuverStartCoords { get; }
}