using Client.GameScreens.Combat.GameObjects;

using CombatDicesTeam.Combats;

namespace Client.GameScreens.Combat.Ui;

internal interface ITargetMarkerContext
{
    ICombatant CurrentCombatant { get; }

    ITargetSelectorContext TargetSelectorContext { get; }
    CombatantGameObject GetCombatantGameObject(ICombatant combatant);
}