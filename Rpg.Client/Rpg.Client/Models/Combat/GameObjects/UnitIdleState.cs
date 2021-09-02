using Microsoft.Xna.Framework;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal class UnitIdleState : IUnitStateEngine
    {
        public UnitIdleState(UnitGraphics unitGraphics)
        {
            unitGraphics.PlayAnimation("Idle");
        }

        /// <inheritdoc />
        /// <remarks> The state engine has no blockers. So we can't remove it with no aftermaths. </remarks>
        public bool CanBeReplaced => true;

        /// <summary>
        /// This engine is infinite.
        /// </summary>
        public bool IsComplete => false;

        public void Cancel()
        {
            // There is no blockers. So do nothing.
        }

        public void Update(GameTime gameTime)
        {
            // TODO Run idle animation
        }
    }
}