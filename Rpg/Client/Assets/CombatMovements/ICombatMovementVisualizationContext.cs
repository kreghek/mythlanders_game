using Client.Assets.States.Primitives;
using Client.GameScreens;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementVisualizationContext
{
    CombatantGameObject ActorGameObject { get; }
    GameObjectContentStorage GameObjectContentStorage { get; }

    InteractionDeliveryManager InteractionDeliveryManager { get; }
    CombatantGameObject GetCombatActor(Combatant combatant);
}