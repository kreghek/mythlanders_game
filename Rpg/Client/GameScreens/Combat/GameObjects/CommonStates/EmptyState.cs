using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects.CommonStates
{
    internal class EmptyState : IActorVisualizationState
    {
        private readonly AnimationBlocker _mainStateBlocker;

        public EmptyState(AnimationBlocker mainStateBlocker)
        {
            _mainStateBlocker = mainStateBlocker;
        }

        public bool CanBeReplaced => true;

        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            _mainStateBlocker.Release();
        }

        public void Update(GameTime gameTime)
        {
            if (!IsComplete)
            {
                IsComplete = true;
                _mainStateBlocker.Release();
            }
        }
    }
}