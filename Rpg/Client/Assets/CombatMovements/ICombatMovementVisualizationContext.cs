using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementVisualizationContext
{
    CombatantGameObject GetCombatActor(Combatant combatant);
}