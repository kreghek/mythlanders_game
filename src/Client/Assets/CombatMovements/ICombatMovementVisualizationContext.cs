﻿using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Core;
using Client.GameScreens;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;
using Core.Dices;

namespace Client.Assets.CombatMovements;

/// <summary>
/// Services and cobat movement context.
/// </summary>
internal interface ICombatMovementVisualizationContext
{
    CombatantGameObject ActorGameObject { get; }
    GameObjectContentStorage GameObjectContentStorage { get; }

    InteractionDeliveryManager InteractionDeliveryManager { get; }
    CombatantGameObject GetCombatActor(Combatant combatant);

    IBattlefieldInteractionContext BattlefieldInteractionContext { get; }
    IDice Dice { get; }
}