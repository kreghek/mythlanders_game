using Microsoft.Xna.Framework;

namespace Rpg.Client.GameScreens.Combat.GameObjects.CommonStates.Primitives
{
    internal class IdleState: IUnitStateEngine
    {
        private double _durationSeconds;

        public IdleState(double durationSeconds)
        {
            _durationSeconds = durationSeconds;
        }

        public bool CanBeReplaced => true;

        public bool IsComplete => _durationSeconds <= 0;

        public void Cancel()
        {
            _durationSeconds = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            _durationSeconds -= gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
