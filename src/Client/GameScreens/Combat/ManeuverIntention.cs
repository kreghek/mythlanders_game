using CombatDicesTeam.Combats;

namespace Client.GameScreens.Combat;

internal sealed class ManeuverIntention : IIntention
{
    private readonly CombatStepDirection _combatStepDirection;

    public ManeuverIntention(CombatStepDirection combatStepDirection)
    {
        _combatStepDirection = combatStepDirection;
    }

    public void Make(CombatEngineBase combatEngine)
    {
        combatEngine.UseManeuver(_combatStepDirection);
    }
}