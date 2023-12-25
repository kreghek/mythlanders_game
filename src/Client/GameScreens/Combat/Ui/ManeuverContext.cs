using System.Linq;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.Ui;

internal sealed class ManeuverContext : IManeuverContext
{
    private readonly CombatEngineBase _combatCore;

    public ManeuverContext(CombatEngineBase combatCore)
    {
        _combatCore = combatCore;
    }

    public CombatFieldSide FieldSide => _combatCore.Field.HeroSide;

    public int? ManeuversAvailableCount =>
        _combatCore.CurrentCombatant?.Stats.Single(x => x.Type == CombatantStatTypes.Maneuver).Value.Current;

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