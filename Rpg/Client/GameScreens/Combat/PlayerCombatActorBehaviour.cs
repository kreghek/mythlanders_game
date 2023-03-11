using System;

using Core.Combats;

namespace Rpg.Client.GameScreens.Combat
{
    internal sealed class PlayerCombatActorBehaviour : ICombatActorBehaviour
    {
        private Action<IIntention>? _intentionDelegate;

        public void Assign(IIntention intention)
        {
            if (_intentionDelegate is null)
            {
                throw new InvalidOperationException();
            }

            _intentionDelegate(intention);
        }

        public void HandleIntention(ICombatActorBehaviourData combatData, Action<IIntention> intentionDelegate)
        {
            _intentionDelegate = intentionDelegate;
        }
    }
}