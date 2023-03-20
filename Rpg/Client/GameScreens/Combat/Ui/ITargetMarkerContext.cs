using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

namespace Client.GameScreens.Combat.Ui;

internal interface ITargetMarkerContext
{
    Combatant CurrentCombatant { get; }

    ITargetSelectorContext TargetSelectorContext { get; }
    CombatantGameObject GetCombatantGameObject(Combatant combatant);
}