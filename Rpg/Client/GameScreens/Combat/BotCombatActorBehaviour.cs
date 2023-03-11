using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat
{
    internal sealed class BotCombatActorBehaviour : ICombatActorBehaviour
    {
        private readonly IAnimationManager _animationManager;
        private readonly ICombatMovementVisualizer _combatMovementVisualizer;
        private readonly IList<CombatantGameObject> _combatantGameObjects;

        public BotCombatActorBehaviour(IAnimationManager animationManager, ICombatMovementVisualizer combatMovementVisualizer, IList<CombatantGameObject> combatantGameObjects)
        {
            _animationManager = animationManager;
            _combatMovementVisualizer = combatMovementVisualizer;
            _combatantGameObjects = combatantGameObjects;
        }

        public void HandleIntention(ICombatActorBehaviourData combatData, Action<IIntention> intentionDelegate)
        {
            var firstSkill = combatData.CurrentActor.Skills.First();

            var skillIntention = new UseCombatMovementIntention(firstSkill.CombatMovement, _animationManager, _combatMovementVisualizer, _combatantGameObjects);

            intentionDelegate(skillIntention);
        }
    }
}