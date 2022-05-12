using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States
{
    internal class SequentialState : IUnitStateEngine
    {
        private readonly IUnitStateEngine[] _subStates;
        private int _subStateIndex;

        public SequentialState(IReadOnlyList<IUnitStateEngine> subStates)
        {
            _subStates = subStates.ToArray();
        }

        public bool CanBeReplaced => true;

        public bool IsComplete { get; private set; }

        public void Cancel()
        { 
            // Nothing to release.
        }

        public void Update(GameTime gameTime)
        {
            if (_subStateIndex < _subStates.Length)
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
