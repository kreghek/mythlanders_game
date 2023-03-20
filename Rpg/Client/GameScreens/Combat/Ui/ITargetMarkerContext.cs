using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

namespace Client.GameScreens.Combat.Ui;

internal interface ITargetMarkerContext
{
    CombatantGameObject GetCombatantGameObject(Combatant combatant);

    Combatant CurrentCombatant { get; }

    ITargetSelectorContext TargetSelectorContext { get; }
}