﻿using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Rpg.Client.GameScreens.Combat.GameObjects.CommonStates
{
    internal sealed class SequentialState : IUnitStateEngine
    {
        private readonly IReadOnlyList<IUnitStateEngine> _subStates;
        private int _subStateIndex;

        public SequentialState(IReadOnlyList<IUnitStateEngine> subStates)
        {
            _subStates = subStates;
        }

        public SequentialState(params IUnitStateEngine[] subStates)
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