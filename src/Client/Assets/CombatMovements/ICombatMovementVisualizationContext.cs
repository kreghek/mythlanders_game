using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Core;
using Client.GameScreens;
using Client.GameScreens.Combat.GameObjects;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Dices;

using GameClient.Engine.CombatVisualEffects;

namespace Client.Assets.CombatMovements;

/// <summary>
/// Services and cobat movement context.
/// </summary>
internal interface ICombatMovementVisualizationContext
{
    CombatantGameObject ActorGameObject { get; }

    IBattlefieldInteractionContext BattlefieldInteractionContext { get; }
    ICombatVisualEffectManager CombatVisualEffectManager { get; }
    IDice Dice { get; }
    GameObjectContentStorage GameObjectContentStorage { get; }

    InteractionDeliveryManager InteractionDeliveryManager { get; }
    CombatantGameObject GetCombatActor(ICombatant combatant);
}