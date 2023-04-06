using Client.Assets.States.Primitives;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Rpg.Client.GameScreens;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementVisualizationContext
{
    CombatantGameObject ActorGameObject { get; }
    GameObjectContentStorage GameObjectContentStorage { get; }

    InteractionDeliveryManager InteractionDeliveryManager { get; }
    CombatantGameObject GetCombatActor(Combatant combatant);
}