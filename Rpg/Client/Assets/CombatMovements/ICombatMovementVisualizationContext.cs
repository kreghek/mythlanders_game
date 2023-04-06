using Client.Assets.States.Primitives;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Rpg.Client.GameScreens;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementVisualizationContext
{
    CombatantGameObject GetCombatActor(Combatant combatant);

    InteractionDeliveryManager InteractionDeliveryManager { get; }
    GameObjectContentStorage GameObjectContentStorage { get; }
    CombatantGameObject ActorGameObject { get; }
}