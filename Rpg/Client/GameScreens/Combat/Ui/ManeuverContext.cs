using System.Linq;

using Core.Combats;

namespace Client.GameScreens.Combat.Ui;

internal sealed class ManeuverContext : IManeuverContext
{
    private readonly CombatCore _combatCore;
    public CombatFieldSide FieldSide => _combatCore.Field.HeroSide;

    public ManeuverContext(CombatCore combatCore)
    {
        _combatCore = combatCore;
    }

    public int? ManeuversAvailable => _combatCore.CurrentCombatant?.Stats.Single(x => x.Type == UnitStatType.Maneuver).Value.Current;

    public FieldCoords? ManeuverStartCoords
    {
        get
        {
            if (_combatCore.CurrentCombatant is null)
            {
                return null;
            }

            return FieldSide.GetCombatantCoords(_combatCore.CurrentCombatant);
        }
    }

}