using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.Ui;

internal interface ITargetMarkerContext
{
    ICombatant CurrentCombatant { get; }

    ITargetSelectorContext TargetSelectorContext { get; }
    CombatantGameObject GetCombatantGameObject(ICombatant combatant);
}