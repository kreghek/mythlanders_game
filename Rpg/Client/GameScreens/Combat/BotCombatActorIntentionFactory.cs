using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;
using Core.Combats.BotBehaviour;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat;

namespace Client.GameScreens.Combat;

internal sealed class BotCombatActorIntentionFactory : IIntentionFactory
{
    private readonly IAnimationManager _animationManager;
    private readonly ICombatMovementVisualizer _combatMovementVisualizer;
    private readonly IList<CombatantGameObject> _combatantGameObjects;

    public BotCombatActorIntentionFactory(IAnimationManager animationManager, ICombatMovementVisualizer combatMovementVisualizer, IList<CombatantGameObject> combatantGameObjects)
    {
        _animationManager = animationManager;
        _combatMovementVisualizer = combatMovementVisualizer;
        _combatantGameObjects = combatantGameObjects;
    }

    public IIntention CreateCombatMovement(CombatMovementInstance combatMovement)
    {
        return new UseCombatMovementIntention(combatMovement, _animationManager, _combatMovementVisualizer, _combatantGameObjects);
    }
}