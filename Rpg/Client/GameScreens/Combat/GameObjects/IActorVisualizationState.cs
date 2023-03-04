using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    public interface IActorVisualizationState
    {
        bool CanBeReplaced { get; }
        bool IsComplete { get; }
        void Cancel();
        void Update(GameTime gameTime);
    }

    public sealed class DelayActorState : IActorVisualizationState
    {
        private double _counterSeconds;

        public DelayActorState(Duration duration)
        {
            _counterSeconds = duration.Seconds;
        }

        public bool CanBeReplaced => true;

        public bool IsComplete => _counterSeconds <= 0;

        public void Cancel()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            _counterSeconds -= gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public record Duration(double Seconds);

    public sealed class SequentialState : IActorVisualizationState
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

    public class AnimationBlockerTerminatorActorState : IActorVisualizationState
    {
        private readonly IAnimationBlocker _animationBlocker;

        public AnimationBlockerTerminatorActorState(IAnimationBlocker animationBlocker)
        {
            _animationBlocker = animationBlocker;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            if (IsComplete)
            {
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            _animationBlocker.Release();
            IsComplete = true;
        }
    }
}