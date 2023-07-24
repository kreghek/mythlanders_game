using Core.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat;

internal sealed class ManeuverIntention : IIntention
{
    private readonly TestamentCombatEngine _combatEngine;
    private readonly CombatStepDirection _combatStepDirection;

    public ManeuverIntention(TestamentCombatEngine combatEngine, CombatStepDirection combatStepDirection)
    {
        _combatEngine = combatEngine;
        _combatStepDirection = combatStepDirection;
    }

    public void Make()
    {
        _combatEngine.UseManeuver(_combatStepDirection);
    }
}