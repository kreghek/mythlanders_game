using Core.Combats;

namespace Rpg.Client.GameScreens.Combat;

internal sealed class ManeverIntention : IIntention
{
    private readonly CombatStepDirection _combatStepDirection;

    public ManeverIntention(CombatStepDirection combatStepDirection)
    {
        _combatStepDirection = combatStepDirection;
    }

    public void Make(CombatCore combatCore)
    {
        combatCore.UseManeuver(_combatStepDirection);
    }
}
