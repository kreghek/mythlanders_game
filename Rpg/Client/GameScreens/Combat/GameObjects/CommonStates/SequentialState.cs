using System.Collections.Generic;

using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

namespace Rpg.Client.GameScreens.Combat.GameObjects.CommonStates
{
    internal sealed class SequentialState : IActorVisualizationState
    {
        private readonly IReadOnlyList<IActorVisualizationState> _subStates;
        private int _subStateIndex;

        public SequentialState(IReadOnlyList<IActorVisualizationState> subStates)
        {
            _subStates = subStates;
        }

        public SequentialState(params IActorVisualizationState[] subStates)
        {
            _subStates = subStates;
        }

        public bool CanBeReplaced => true;

        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            // Nothing to release.
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            if (_subStateIndex < _subStates.Count)
            {
                var currentSubState = _subStates[_subStateIndex];
                if (currentSubState.IsComplete)
                {
                    _subStateIndex++;
                }
                else
                {
                    currentSubState.Update(gameTime);
                }
            }
            else
            {
                IsComplete = true;
            }
        }
    }
}