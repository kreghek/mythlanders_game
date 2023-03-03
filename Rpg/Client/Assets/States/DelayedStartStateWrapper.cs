using Microsoft.Xna.Framework;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States
{
    internal sealed class DelayedStartStateWrapper : IActorVisualizationState
    {
        private readonly IActorVisualizationState _mainState;
        private double _delayCounter;

        public DelayedStartStateWrapper(IActorVisualizationState mainState, float delayDurationSeconds = 0f)
        {
            _mainState = mainState;
            _delayCounter = delayDurationSeconds;
        }

        public bool CanBeReplaced => true;

        public bool IsComplete => _mainState.IsComplete;

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

            if (_delayCounter > 0)
            {
                _delayCounter -= gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _mainState.Update(gameTime);
            }
        }
    }
}